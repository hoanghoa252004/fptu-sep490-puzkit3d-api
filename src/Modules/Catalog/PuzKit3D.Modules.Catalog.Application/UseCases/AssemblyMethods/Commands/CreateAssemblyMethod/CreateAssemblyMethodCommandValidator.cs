using FluentValidation;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Commands.CreateAssemblyMethod;

internal sealed class CreateAssemblyMethodCommandValidator : AbstractValidator<CreateAssemblyMethodCommand>
{
    public CreateAssemblyMethodCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Assembly method name is required")
            .MaximumLength(30).WithMessage("Assembly method name must not exceed 30 characters");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Assembly method slug is required")
            .MaximumLength(30).WithMessage("Assembly method slug must not exceed 30 characters")
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be lowercase alphanumeric with hyphens only");
    }
}

