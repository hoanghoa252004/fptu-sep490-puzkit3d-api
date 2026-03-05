using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure<string>(
                Error.Failure("Authentication.RegistrationFailed", errors));
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

        if (!roleResult.Succeeded)
        {
            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            return Result.Failure<string>(
                Error.Failure("Authentication.RoleAssignmentFailed", errors));
        }

        return Result.Success($"User registered successfully with email {email}");
    }

    public async Task<ResultT<AuthenticationResult>> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null || user.IsDeleted)
        {
            return Result.Failure<AuthenticationResult>(
                Error.Unauthorized("Authentication.InvalidCredentials", "Invalid email or password"));
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            return Result.Failure<AuthenticationResult>(
                Error.Forbidden("Authentication.AccountLocked", "Your account has been locked. Please contact administrator."));
        }

        if (!result.Succeeded)
        {
            return Result.Failure<AuthenticationResult>(
                Error.Unauthorized("Authentication.InvalidCredentials", "Invalid email or password"));
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
        var allowedRoles = new[] { "Staff",  "Business Manager" };
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

    public async Task<ResultT<AuthenticationResult>> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);

        if (user is null || 
            user.IsDeleted || 
            user.RefreshTokenExpiryTime is null || 
            user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Result.Failure<AuthenticationResult>(
                Error.Unauthorized("Authentication.InvalidRefreshToken", "Invalid or expired refresh token"));
        }

        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
        {
            return Result.Failure<AuthenticationResult>(
                Error.Forbidden("Authentication.AccountLocked", "Your account has been locked"));
        }

        var tokenResult = await _jwtProvider.GenerateTokenAsync(user.Id, user.Email!, cancellationToken);

        if (tokenResult.IsFailure)
        {
            return Result.Failure<AuthenticationResult>(tokenResult.Error);
        }

        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(user);

        return Result.Success(new AuthenticationResult(
            user.Id,
            user.Email!,
            tokenResult.Value,
            newRefreshToken,
            DateTime.UtcNow.AddMinutes(60)));
    }

    public async Task<Result> LogoutAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result.Failure(Error.NotFound("Authentication.UserNotFound", "User not found"));
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _userManager.UpdateAsync(user);

        return Result.Success();
    }

    public async Task<ResultT<object>> GetUsersAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = _userManager.Users
            .Where(u => !u.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(u =>
                (u.Email != null && u.Email.Contains(searchTerm)) ||
                (u.FirstName != null && u.FirstName.Contains(searchTerm)) ||
                (u.LastName != null && u.LastName.Contains(searchTerm)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.FirstName,
                u.LastName,
                u.PhoneNumber,
                u.EmailConfirmed,
                u.LockoutEnd,
                IsLocked = u.LockoutEnd.HasValue && u.LockoutEnd > DateTimeOffset.UtcNow,
                u.CreatedAt,
                u.UpdatedAt,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            })
            .ToListAsync(cancellationToken);

        var result = new
        {
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            Users = users
        };

        return Result.Success<object>(result);
    }

    public async Task<ResultT<object>> GetUserByIdAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .Where(u => u.Id == userId && !u.IsDeleted)
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.FirstName,
                u.LastName,
                u.PhoneNumber,
                u.EmailConfirmed,
                u.LockoutEnd,
                IsLocked = u.LockoutEnd.HasValue && u.LockoutEnd > DateTimeOffset.UtcNow,
                u.CreatedAt,
                u.UpdatedAt,
                u.ProvinceId,
                u.ProvinceName,
                u.DistrictId,
                u.DistrictName,
                u.WardCode,
                u.WardName,
                u.StreetAddress,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure<object>(
                Error.NotFound("User.NotFound", $"User with ID {userId} not found"));
        }

        return Result.Success<object>(user);
    }

    public async Task<Result> UpdateUserAsync(
        string userId,
        string? firstName,
        string? lastName,
        string? phoneNumber,
        string? provinceId,
        string? provinceName,
        string? districtId,
        string? districtName,
        string? wardCode,
        string? wardName,
        string? streetAddress,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null || user.IsDeleted)
        {
            return Result.Failure(
                Error.NotFound("User.NotFound", $"User with ID {userId} not found"));
        }

        user.FirstName = firstName;
        user.LastName = lastName;
        user.PhoneNumber = phoneNumber;
        user.ProvinceId = provinceId;
        user.ProvinceName = provinceName;
        user.DistrictId = districtId;
        user.DistrictName = districtName;
        user.WardCode = wardCode;
        user.WardName = wardName;
        user.StreetAddress = streetAddress;
        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure(
                Error.Failure("User.UpdateFailed", errors));
        }

        return Result.Success();
    }

    public async Task<Result> DeleteUserAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null || user.IsDeleted)
        {
            return Result.Failure(
                Error.NotFound("User.NotFound", $"User with ID {userId} not found"));
        }

        user.IsDeleted = true;
        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure(
                Error.Failure("User.DeleteFailed", errors));
        }

        return Result.Success();
    }

    public async Task<Result> LockUserAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null || user.IsDeleted)
        {
            return Result.Failure(
                Error.NotFound("User.NotFound", $"User with ID {userId} not found"));
        }

        var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure(
                Error.Failure("User.LockFailed", errors));
        }

        return Result.Success();
    }

    public async Task<Result> UnlockUserAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null || user.IsDeleted)
        {
            return Result.Failure(
                Error.NotFound("User.NotFound", $"User with ID {userId} not found"));
        }

        var result = await _userManager.SetLockoutEndDateAsync(user, null);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure(
                Error.Failure("User.UnlockFailed", errors));
        }

        return Result.Success();
    }

    public async Task<Result> ChangeUserRoleAsync(
        string userId,
        string newRole,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null || user.IsDeleted)
        {
            return Result.Failure(
                Error.NotFound("User.NotFound", $"User with ID {userId} not found"));
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

        if (!removeResult.Succeeded)
        {
            var errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));
            return Result.Failure(
                Error.Failure("User.RoleRemovalFailed", errors));
        }

        var addResult = await _userManager.AddToRoleAsync(user, newRole);

        if (!addResult.Succeeded)
        {
            var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
            return Result.Failure(
                Error.Failure("User.RoleAssignmentFailed", errors));
        }

        return Result.Success();
    }

    public async Task<ResultT<object>> GetProfileAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .Where(u => u.Id == userId && !u.IsDeleted)
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.FirstName,
                u.LastName,
                u.PhoneNumber,
                u.EmailConfirmed,
                u.ProvinceId,
                u.ProvinceName,
                u.DistrictId,
                u.DistrictName,
                u.WardCode,
                u.WardName,
                u.StreetAddress,
                u.CreatedAt,
                u.UpdatedAt,
                Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            return Result.Failure<object>(
                Error.NotFound("User.NotFound", "User profile not found"));
        }

        return Result.Success<object>(user);
    }

    public async Task<Result> UpdateProfileAsync(
        string userId,
        string? firstName,
        string? lastName,
        string? phoneNumber,
        string? provinceId,
        string? provinceName,
        string? districtId,
        string? districtName,
        string? wardCode,
        string? wardName,
        string? streetAddress,
        CancellationToken cancellationToken = default)
    {
        return await UpdateUserAsync(
            userId,
            firstName,
            lastName,
            phoneNumber,
            provinceId,
            provinceName,
            districtId,
            districtName,
            wardCode,
            wardName,
            streetAddress,
            cancellationToken);
    }

    public async Task<Result> UpdateAvatarAsync(
        string userId,
        string avatarUrl,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null || user.IsDeleted)
        {
            return Result.Failure(
                Error.NotFound("User.NotFound", "User not found"));
        }

        // TODO: Add Avatar property to ApplicationUser model if needed
        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure(
                Error.Failure("User.UpdateFailed", errors));
        }

        return Result.Success();
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}

