using InventoryApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(e =>
        {
            e.Property(p => p.Name).IsRequired().HasMaxLength(200);
            e.Property(p => p.Sku).IsRequired().HasMaxLength(50);
            e.HasIndex(p => p.Sku).IsUnique();
            e.Property(p => p.Price).HasPrecision(18, 2);
        });

        // Seed data so GET returns something on first run
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Wireless Mouse", Sku = "WM-001", Price = 4500m, QuantityInStock = 120, ReorderLevel = 20 },
            new Product { Id = 2, Name = "Mechanical Keyboard", Sku = "MK-002", Price = 18500m, QuantityInStock = 45, ReorderLevel = 10 },
            new Product { Id = 3, Name = "USB-C Hub", Sku = "UH-003", Price = 9200m, QuantityInStock = 8, ReorderLevel = 15 }
        );
    }
}