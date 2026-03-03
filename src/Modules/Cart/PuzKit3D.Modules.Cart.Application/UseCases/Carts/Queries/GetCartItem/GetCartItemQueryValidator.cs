using FluentValidation;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Queries.GetCartItem;

internal sealed class GetCartItemQueryValidator : AbstractValidator<GetCartItemQuery>
{
    public GetCartItemQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");

        RuleFor(x => x.CartType)
            .NotEmpty()
            .WithMessage("Cart type is required")
            .Must(type => type.ToUpper() == "INSTOCK" || type.ToUpper() == "PARTNER")
            .WithMessage("Cart type must be 'INSTOCK' or 'PARTNER'");

        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item ID is required");
    }
}
