using InventoryApi.Application;
using InventoryApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Infrastructure;

public class ProductRepository(AppDbContext db) : IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetAllAsync()
        => await db.Products.AsNoTracking().ToListAsync();

    public async Task<Product?> GetByIdAsync(int id)
        => await db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Product> AddAsync(Product product)
    {
        db.Products.Add(product);
        await db.SaveChangesAsync();
        return product;
    }
}