using InventoryApi.Domain;

namespace InventoryApi.Application;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> AddAsync(Product product);
}