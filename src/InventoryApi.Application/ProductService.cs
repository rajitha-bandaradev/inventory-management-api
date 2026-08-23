using InventoryApi.Domain;

namespace InventoryApi.Application;

public class ProductService(IProductRepository repo)
{
    public async Task<IReadOnlyList<Product>> GetLowStockProductsAsync()
        => (await repo.GetAllAsync()).Where(p => p.IsLowStock).ToList();
}