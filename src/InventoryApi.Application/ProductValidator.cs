using FluentValidation;
using InventoryApi.Domain;

namespace InventoryApi.Application;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200);

        RuleFor(p => p.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(50)
            .Matches("^[A-Z0-9-]+$").WithMessage("SKU must contain only uppercase letters, digits, and hyphens.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(p => p.QuantityInStock)
            .GreaterThanOrEqualTo(0);

        RuleFor(p => p.ReorderLevel)
            .GreaterThanOrEqualTo(0);
    }
}