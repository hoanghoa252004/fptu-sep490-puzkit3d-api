using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Authorization;


/// <summary>
/// Service for managing user permissions
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Checks if the current user has a specific permission
    /// </summary>
    Task<bool> HasPermissionAsync(
        string permission,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a specific user has a permission
    /// </summary>
    Task<bool> HasPermissionAsync(
        string userId,
        string permission,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all permissions for the current user
    /// </summary>
    Task<HashSet<string>> GetPermissionsAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all permissions for a specific user
    /// </summary>
    Task<HashSet<string>> GetPermissionsAsync(
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Assigns permissions to a user
    /// </summary>
    Task<bool> AssignPermissionsAsync(
        string userId,
        IEnumerable<string> permissions,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes permissions from a user
    /// </summary>
    Task<bool> RemovePermissionsAsync(
        string userId,
        IEnumerable<string> permissions,
        CancellationToken cancellationToken = default);
}