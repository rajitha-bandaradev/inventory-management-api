using InventoryApi.Application;
using InventoryApi.Domain;

namespace InventoryApi.Tests;

public class ProductValidatorTests
{
    private readonly ProductValidator _validator = new();

    private static Product ValidProduct() => new()
    {
        Name = "Test Product",
        Sku = "TP-001",
        Price = 100m,
        QuantityInStock = 10,
        ReorderLevel = 5
    };

    [Fact]
    public void Valid_product_passes_validation()
    {
        var result = _validator.Validate(ValidProduct());
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Empty_name_fails_validation()
    {
        var product = ValidProduct();
        product.Name = "";
        var result = _validator.Validate(product);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(Product.Name));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Non_positive_price_fails_validation(decimal price)
    {
        var product = ValidProduct();
        product.Price = price;
        Assert.False(_validator.Validate(product).IsValid);
    }

    [Fact]
    public void Lowercase_sku_fails_validation()
    {
        var product = ValidProduct();
        product.Sku = "tp-001";
        Assert.False(_validator.Validate(product).IsValid);
    }
}