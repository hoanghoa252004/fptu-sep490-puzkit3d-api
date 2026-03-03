using FluentValidation;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.UpdateCartItem;

internal sealed class UpdateCartItemCommandValidator : AbstractValidator<UpdateCartItemCommand>
{
    public UpdateCartItemCommandValidator()
    {
        RuleFor(x => x.ItemType)
            .NotEmpty()
            .WithMessage("Item type is required")
            .Must(type => type.ToUpper() == "INSTOCK" || type.ToUpper() == "PARTNER")
            .WithMessage("Item type must be 'INSTOCK' or 'PARTNER'");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
    }
}
