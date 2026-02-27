using Microsoft.AspNetCore.Identity;
using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.SharedKernel.Infrastructure.Identity;
using System.Security.Cryptography;

namespace PuzKit3D.SharedKernel.Infrastructure.Authentication;

public sealed class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtProvider _jwtProvider;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtProvider = jwtProvider;
    }

    public async Task<ResultT<string>> RegisterAsync(
        string email,
        string password,
        string? firstName,
        string? lastName,
        CancellationToken cancellationToken = default)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser is not null)
        {
            return Result.Failure<string>(
                Error.Conflict("Authentication.EmailAlreadyExists", $"User with email {email} already exists"));
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure<string>(
                Error.Failure("Authentication.RegistrationFailed", errors));
        }

        // Add default role
        await _userManager.AddToRoleAsync(user, "Customer");

        return Result.Success($"User with email {email} is created successfully");
    }

    public async Task<ResultT<AuthenticationResult>> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result.Failure<AuthenticationResult>(
                Error.NotFound("Authentication.InvalidCredentials", $"User with email {email} does not exist"));
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            return Result.Failure<AuthenticationResult>(
                Error.Unauthorized("Authentication.InvalidCredentials", "Incorrect email or password"));
        }

        var tokenResult = await _jwtProvider.GenerateTokenAsync(user.Id, user.Email!, cancellationToken);

        if (tokenResult.IsFailure)
        {
            return Result.Failure<AuthenticationResult>(tokenResult.Error);
        }

        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return Result.Success(new AuthenticationResult(
            user.Id,
            user.Email!,
            tokenResult.Value,
            refreshToken,
            DateTime.UtcNow.AddMinutes(60)));
    }

    public async Task<Result> ConfirmEmailAsync(
        string userId,
        string token,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(Error.Failure("Authentication.UserNotFound", "User not found"));
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
        {
            return Result.Failure(Error.Failure("Authentication.EmailConfirmationFailed", "Email confirmation failed"));
        }

        return Result.Success();
    }

    public async Task<Result> ForgotPasswordAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            // Don't reveal that the user does not exist
            return Result.Success();
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        // TODO: Send email with token

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(
        string userId,
        string token,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(Error.Failure("Authentication.UserNotFound", "User not found"));
        }

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure(Error.Failure("Authentication.PasswordResetFailed", errors));
        }

        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(
        string userId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(Error.Failure("Authentication.UserNotFound", "User not found"));
        }

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure(Error.Failure("Authentication.PasswordChangeFailed", errors));
        }

        return Result.Success();
    }

    public async Task<ResultT<string>> CreateUserWithRoleAsync(
        string email,
        string password,
        string role,
        string? firstName,
        string? lastName,
        CancellationToken cancellationToken = default)
    {
        // Validate role (only allow Staff or Manager)
        var allowedRoles = new[] { "Staff", "Manager", "Business Manager" };
        if (!allowedRoles.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase)))
        {
            return Result.Failure<string>(
                Error.Validation("Authentication.InvalidRole", $"Role '{role}' is not allowed. Only Staff or Manager roles can be created."));
        }

        // Check if user already exists
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser is not null)
        {
            return Result.Failure<string>(
                Error.Conflict("Authentication.EmailAlreadyExists", $"User with email {email} already exists"));
        }

        // Create user
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure<string>(
                Error.Failure("Authentication.UserCreationFailed", errors));
        }

        // Assign role
        var roleResult = await _userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded)
        {
            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            return Result.Failure<string>(
                Error.Failure("Authentication.RoleAssignmentFailed", errors));
        }

        return Result.Success($"User with email {email} created successfully with role {role}");
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}

