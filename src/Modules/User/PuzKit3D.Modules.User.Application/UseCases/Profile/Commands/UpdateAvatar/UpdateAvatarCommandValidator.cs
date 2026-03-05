using FluentValidation;

namespace PuzKit3D.Modules.User.Application.UseCases.Profile.Commands.UpdateAvatar;

internal sealed class UpdateAvatarCommandValidator : AbstractValidator<UpdateAvatarCommand>
{
    public UpdateAvatarCommandValidator()
    {
        RuleFor(x => x.AvatarUrl)
            .NotEmpty()
            .WithMessage("Avatar URL is required")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Avatar URL must be a valid URL");
    }
}
