using FluentValidation;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAllAssemblyMethods;

internal sealed class GetAllAssemblyMethodsQueryValidator : AbstractValidator<GetAllAssemblyMethodsQuery>
{
    public GetAllAssemblyMethodsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1");

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100");
    }
}

