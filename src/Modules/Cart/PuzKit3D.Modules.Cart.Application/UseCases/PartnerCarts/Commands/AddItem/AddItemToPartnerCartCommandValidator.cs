using FluentValidation;

namespace PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Commands.AddItem;

internal sealed class AddItemToPartnerCartCommandValidator : AbstractValidator<AddItemToPartnerCartCommand>
{
    public AddItemToPartnerCartCommandValidator()
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
