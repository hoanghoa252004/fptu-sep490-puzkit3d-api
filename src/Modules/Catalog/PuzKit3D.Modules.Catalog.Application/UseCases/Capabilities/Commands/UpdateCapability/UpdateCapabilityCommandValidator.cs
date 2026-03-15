using FluentValidation;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Commands.UpdateCapability;

internal sealed class UpdateCapabilityCommandValidator : AbstractValidator<UpdateCapabilityCommand>
{
    public UpdateCapabilityCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Capability ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Capability name is required")
            .MaximumLength(30).WithMessage("Capability name must not exceed 30 characters")
            .When(x => x.Name != null);

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Capability slug is required")
            .MaximumLength(30).WithMessage("Capability slug must not exceed 30 characters")
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be lowercase alphanumeric with hyphens only")
            .When(x => x.Slug != null);
    }
}
