using FluentValidation;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Commands.UpdateAssemblyMethod;

internal sealed class UpdateAssemblyMethodCommandValidator : AbstractValidator<UpdateAssemblyMethodCommand>
{
    public UpdateAssemblyMethodCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Assembly method ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Assembly method name is required")
            .MaximumLength(30).WithMessage("Assembly method name must not exceed 30 characters");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Assembly method slug is required")
            .MaximumLength(30).WithMessage("Assembly method slug must not exceed 30 characters")
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be lowercase alphanumeric with hyphens only");
    }
}

