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
}