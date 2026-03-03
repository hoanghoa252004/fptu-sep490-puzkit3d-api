using FluentValidation;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.RemoveCartItem;

internal sealed class RemoveCartItemCommandValidator : AbstractValidator<RemoveCartItemCommand>
{
    public RemoveCartItemCommandValidator()
    {
        RuleFor(x => x.ItemType)
            .NotEmpty()
            .WithMessage("Item type is required")
            .Must(type => type.ToUpper() == "INSTOCK" || type.ToUpper() == "PARTNER")
            .WithMessage("Item type must be 'INSTOCK' or 'PARTNER'");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required");
    }
}
