using FluentValidation;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.AddItemToCart;

internal sealed class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(x => x.ItemType)
            .NotEmpty()
            .WithMessage("Item type is required")
            .Must(type => type.ToLower() == "instock" || type.ToLower() == "partner")
            .WithMessage("Item type must be either 'instock' or 'partner'");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .When(x => x.Quantity.HasValue)
            .WithMessage("Quantity must be greater than 0");
    }
}

