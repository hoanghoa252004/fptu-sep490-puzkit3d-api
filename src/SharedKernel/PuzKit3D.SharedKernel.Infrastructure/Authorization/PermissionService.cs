using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Infrastructure.Authorization;

/// <summary>
/// Service for checking user permissions from database
/// </summary>
public sealed class PermissionService : IPermissionService
{
    private readonly ICurrentUser _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IdentityDbContext _context;

    public PermissionService(
        ICurrentUser currentUser,
        UserManager<ApplicationUser> userManager,
        IdentityDbContext context)
    {
        _currentUser = currentUser;
        _userManager = userManager;
        _context = context;
    }

    public async Task<bool> HasPermissionAsync(
        string permission,
        CancellationToken cancellationToken = default)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
        {
            return false;
        }

        return await HasPermissionAsync(_currentUser.UserId, permission, cancellationToken);
    }

    public async Task<bool> HasPermissionAsync(
        string userId,
        string permission,
        CancellationToken cancellationToken = default)
    {
        var permissions = await GetPermissionsAsync(userId, cancellationToken);
        return permissions.Contains(permission);
    }

    public async Task<HashSet<string>> GetPermissionsAsync(
        CancellationToken cancellationToken = default)
    {
        if (!_currentUser.IsAuthenticated || string.IsNullOrWhiteSpace(_currentUser.UserId))
        {
            return [];
        }

        return await GetPermissionsAsync(_currentUser.UserId, cancellationToken);
    }

    public async Task<HashSet<string>> GetPermissionsAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return [];
        }

        // Get user-specific permissions
        var userPermissions = await _context.UserPermissions
            .Where(up => up.UserId == userId)
            .Select(up => up.Permission)
            .ToListAsync(cancellationToken);

        // Get role-based permissions
        var userRoles = await _userManager.GetRolesAsync(user);
        var rolePermissions = await _context.RolePermissions
            .Where(rp => userRoles.Contains(rp.Role.Name!))
            .Select(rp => rp.Permission)
            .ToListAsync(cancellationToken);

        return userPermissions.Concat(rolePermissions).ToHashSet();
    }

    public async Task<bool> AssignPermissionsAsync(
        string userId,
        IEnumerable<string> permissions,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return false;
        }

        var currentUserId = _currentUser.UserId;

        foreach (var permission in permissions)
        {
            if (!await _context.UserPermissions.AnyAsync(
                up => up.UserId == userId && up.Permission == permission,
                cancellationToken))
            {
                _context.UserPermissions.Add(new ApplicationUserPermission
                {
                    UserId = userId,
                    Permission = permission,
                    GrantedAt = DateTime.UtcNow,
                    GrantedBy = currentUserId
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RemovePermissionsAsync(
        string userId,
        IEnumerable<string> permissions,
        CancellationToken cancellationToken = default)
    {
        var userPermissions = await _context.UserPermissions
            .Where(up => up.UserId == userId && permissions.Contains(up.Permission))
            .ToListAsync(cancellationToken);

        _context.UserPermissions.RemoveRange(userPermissions);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}