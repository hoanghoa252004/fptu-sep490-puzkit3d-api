using FluentValidation;

namespace PuzKit3D.Modules.InStock.Application.UserCases.Products.Queries.GetProductById;

internal sealed class GetProductByIdValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");
    }
}
