using FluentValidation;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAssemblyMethodBySlug;

internal sealed class GetAssemblyMethodBySlugQueryValidator : AbstractValidator<GetAssemblyMethodBySlugQuery>
{
    public GetAssemblyMethodBySlugQueryValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Assembly method slug is required")
            .MaximumLength(30).WithMessage("Assembly method slug must not exceed 30 characters");
    }
}
