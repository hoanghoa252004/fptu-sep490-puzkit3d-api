using FluentValidation;

namespace PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Commands.AddItem;

internal sealed class AddItemToInStockCartCommandValidator : AbstractValidator<AddItemToInStockCartCommand>
{
    public AddItemToInStockCartCommandValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .When(x => x.Quantity.HasValue)
            .WithMessage("Quantity must be greater than 0");
    }
}
