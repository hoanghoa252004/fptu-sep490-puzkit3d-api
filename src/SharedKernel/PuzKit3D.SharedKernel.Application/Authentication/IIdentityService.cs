using PuzKit3D.SharedKernel.Application.Authentication.Dtos;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Authentication;

/// <summary>
/// Service for user authentication and registration
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Registers a new user
    /// </summary>
    Task<ResultT<string>> RegisterAsync(
        string email,
        string password,
        string? firstName = null,
        string? lastName = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Authenticates a user with email and password
    /// </summary>
    Task<ResultT<AuthenticationResult>> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirms user email with token
    /// </summary>
    Task<Result> ConfirmEmailAsync(
        string userId,
        string token,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends password reset email
    /// </summary>
    Task<Result> ForgotPasswordAsync(
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets user password with token
    /// </summary>
    Task<Result> ResetPasswordAsync(
        string userId,
        string token,
        string newPassword,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes user password
    /// </summary>
    Task<Result> ChangePasswordAsync(
        string userId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new user with specified role (Admin only)
    /// </summary>
    Task<ResultT<string>> CreateUserWithRoleAsync(
        string email,
        string password,
        string role,
        string? firstName = null,
        string? lastName = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes access token using refresh token
    /// </summary>
    Task<ResultT<AuthenticationResult>> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs out user by invalidating refresh token
    /// </summary>
    Task<Result> LogoutAsync(
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all users (paginated)
    /// </summary>
    Task<ResultT<GetUsersResponse>> GetUsersAsync(
        int pageNumber = 1,
        int pageSize = 10,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets user by ID
    /// </summary>
    Task<ResultT<UserDetailDto>> GetUserByIdAsync(
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes user (soft delete)
    /// </summary>
    Task<Result> DeleteUserAsync(
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Activates (restores) a deleted user
    /// </summary>
    Task<Result> ActivateUserAsync(
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current user profile
    /// </summary>
    Task<ResultT<UserDetailDto>> GetProfileAsync(
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates current user profile
    /// </summary>
    Task<Result> UpdateProfileAsync(
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
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates user avatar
    /// </summary>
    Task<Result> UpdateAvatarAsync(
        string userId,
        string avatarUrl,
        CancellationToken cancellationToken = default);
}
