using InventoryApi.Application;
using InventoryApi.Domain;
using InventoryApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text;
using InventoryApi.Api.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=inventory.db"));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddOpenApi();
builder.Services.AddScoped<FluentValidation.IValidator<Product>, ProductValidator>();
builder.Services.AddScoped<ProductService>();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Apply migrations automatically on startup (fine for a demo API)
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
}

app.MapPost("/api/auth/login", (LoginRequest request, TokenService tokens) =>
{
    // Demo credentials only - a production system would verify hashed passwords from the database
    if (request.Username == "admin" && request.Password == "admin123")
        return Results.Ok(new { token = tokens.CreateToken(request.Username, "Admin") });

    return Results.Unauthorized();
});

app.MapOpenApi();

app.MapGet("/api/products", async (IProductRepository repo)
    => Results.Ok(await repo.GetAllAsync()));

app.MapGet("/api/products/{id:int}", async (int id, IProductRepository repo)
    => await repo.GetByIdAsync(id) is { } product ? Results.Ok(product) : Results.NotFound());

app.MapGet("/api/products/low-stock", async (ProductService service)
    => Results.Ok(await service.GetLowStockProductsAsync()));

app.MapPost("/api/products", async (Product product, FluentValidation.IValidator<Product> validator, IProductRepository repo) =>
{
    var validation = await validator.ValidateAsync(product);
    if (!validation.IsValid)
        return Results.ValidationProblem(validation.ToDictionary());

    var created = await repo.AddAsync(product);
    return Results.Created($"/api/products/{created.Id}", created);
}).RequireAuthorization();

app.MapPut("/api/products/{id:int}", async (int id, Product product,
    FluentValidation.IValidator<Product> validator, IProductRepository repo) =>
{
    if (id != product.Id) return Results.BadRequest("Route id and body id must match.");

    var validation = await validator.ValidateAsync(product);
    if (!validation.IsValid) return Results.ValidationProblem(validation.ToDictionary());

    return await repo.UpdateAsync(product) ? Results.NoContent() : Results.NotFound();
}).RequireAuthorization();

app.MapDelete("/api/products/{id:int}", async (int id, IProductRepository repo)
    => await repo.DeleteAsync(id) ? Results.NoContent() : Results.NotFound()).RequireAuthorization();

app.Run();