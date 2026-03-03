using FluentValidation;

namespace PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Commands.RemoveItem;

internal sealed class RemoveInStockCartItemCommandValidator : AbstractValidator<RemoveInStockCartItemCommand>
{
    public RemoveInStockCartItemCommandValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required");
    }
}
