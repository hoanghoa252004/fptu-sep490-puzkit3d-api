using FluentValidation;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Commands.UpdateTopic;

internal sealed class UpdateTopicCommandValidator : AbstractValidator<UpdateTopicCommand>
{
    public UpdateTopicCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Topic ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Topic name is required")
            .MaximumLength(30).WithMessage("Topic name must not exceed 30 characters")
            .When(x => x.Name != null);

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Topic slug is required")
            .MaximumLength(30).WithMessage("Topic slug must not exceed 30 characters")
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Slug must be lowercase alphanumeric with hyphens only")
            .When(x => x.Slug != null);
    }
}
