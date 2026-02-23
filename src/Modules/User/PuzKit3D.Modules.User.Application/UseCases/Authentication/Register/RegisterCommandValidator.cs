using FluentValidation;

namespace PuzKit3D.Modules.User.Application.UseCases.Authentication.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid")
            .MaximumLength(256).WithMessage("Email can not > 256 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must have at least 8 characters")
            .Matches(@"[A-Z]").WithMessage("Password must have at least 1 uppercase letter")
            .Matches(@"[a-z]").WithMessage("Password must have at least 1 lowercase letter")
            .Matches(@"[0-9]").WithMessage("Password must have at least 1 digit")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must have at least 1 special character");

        RuleFor(x => x.FirstName)
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");
    }
}
