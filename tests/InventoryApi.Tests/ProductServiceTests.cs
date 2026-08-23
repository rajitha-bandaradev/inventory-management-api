using InventoryApi.Application;
using InventoryApi.Domain;
using Moq;

namespace InventoryApi.Tests;

public class ProductServiceTests
{
    [Fact]
    public async Task GetLowStockProducts_returns_only_products_at_or_below_reorder_level()
    {
        // Arrange: mock the repository - no database needed
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Plenty",   QuantityInStock = 100, ReorderLevel = 10 },
            new() { Id = 2, Name = "AtLevel",  QuantityInStock = 10,  ReorderLevel = 10 },
            new() { Id = 3, Name = "Below",    QuantityInStock = 2,   ReorderLevel = 10 },
        };

        var repoMock = new Mock<IProductRepository>();
        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

        var service = new ProductService(repoMock.Object);

        // Act
        var result = await service.GetLowStockProductsAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.DoesNotContain(result, p => p.Id == 1);
        repoMock.Verify(r => r.GetAllAsync(), Times.Once);
    }
}