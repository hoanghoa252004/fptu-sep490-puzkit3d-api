using FluentValidation;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAssemblyMethodById;

internal sealed class GetAssemblyMethodByIdQueryValidator : AbstractValidator<GetAssemblyMethodByIdQuery>
{
    public GetAssemblyMethodByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Assembly method ID is required");
    }
}

