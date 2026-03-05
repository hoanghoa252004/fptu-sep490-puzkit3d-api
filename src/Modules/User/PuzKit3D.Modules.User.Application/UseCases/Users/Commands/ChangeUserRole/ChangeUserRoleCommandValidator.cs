using FluentValidation;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Commands.ChangeUserRole;

internal sealed class ChangeUserRoleCommandValidator : AbstractValidator<ChangeUserRoleCommand>
{
    public ChangeUserRoleCommandValidator()
    {
        RuleFor(x => x.NewRole)
            .NotEmpty()
            .WithMessage("Role is required");
    }
}
