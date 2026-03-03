using FluentValidation;

namespace PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Commands.RemoveItem;

internal sealed class RemovePartnerCartItemCommandValidator : AbstractValidator<RemovePartnerCartItemCommand>
{
    public RemovePartnerCartItemCommandValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required");
    }
}
