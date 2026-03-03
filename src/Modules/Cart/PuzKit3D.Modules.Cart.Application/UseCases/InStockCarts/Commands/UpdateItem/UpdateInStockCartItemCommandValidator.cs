using FluentValidation;

namespace PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Commands.UpdateItem;

internal sealed class UpdateInStockCartItemCommandValidator : AbstractValidator<UpdateInStockCartItemCommand>
{
    public UpdateInStockCartItemCommandValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
    }
}
