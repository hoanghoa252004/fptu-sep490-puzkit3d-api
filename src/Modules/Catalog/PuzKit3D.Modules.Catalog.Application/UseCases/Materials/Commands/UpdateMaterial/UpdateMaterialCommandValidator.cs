using FluentValidation;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Commands.UpdateMaterial;

internal sealed class UpdateMaterialCommandValidator : AbstractValidator<UpdateMaterialCommand>
{
    public UpdateMaterialCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Material ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Material name is required")
            .MaximumLength(30).WithMessage("Material name must not exceed 30 characters")
            .When(x => x.Name != null);

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Material slug is required")
            .MaximumLength(30).WithMessage("Material slug must not exceed 30 characters")
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be lowercase alphanumeric with hyphens only")
            .When(x => x.Slug != null);
    }
}
