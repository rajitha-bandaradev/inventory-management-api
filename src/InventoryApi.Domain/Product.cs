namespace InventoryApi.Domain;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public int ReorderLevel { get; set; }

    public bool IsLowStock => QuantityInStock <= ReorderLevel;
}