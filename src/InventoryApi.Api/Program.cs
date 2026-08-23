using InventoryApi.Application;
using InventoryApi.Domain;
using InventoryApi.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=inventory.db"));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddOpenApi();
builder.Services.AddScoped<FluentValidation.IValidator<Product>, ProductValidator>();
builder.Services.AddScoped<ProductService>();
var app = builder.Build();

// Apply migrations automatically on startup (fine for a demo API)
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
}

app.MapOpenApi();

app.MapGet("/api/products", async (IProductRepository repo)
    => Results.Ok(await repo.GetAllAsync()));

app.MapGet("/api/products/{id:int}", async (int id, IProductRepository repo)
    => await repo.GetByIdAsync(id) is { } product ? Results.Ok(product) : Results.NotFound());

app.MapGet("/api/products/low-stock", async (IProductRepository repo)
    => Results.Ok((await repo.GetAllAsync()).Where(p => p.IsLowStock)));

app.MapPost("/api/products", async (Product product, FluentValidation.IValidator<Product> validator, IProductRepository repo) =>
{
    var validation = await validator.ValidateAsync(product);
    if (!validation.IsValid)
        return Results.ValidationProblem(validation.ToDictionary());

    var created = await repo.AddAsync(product);
    return Results.Created($"/api/products/{created.Id}", created);
});

app.Run();