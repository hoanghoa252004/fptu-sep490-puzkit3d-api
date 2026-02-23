using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Infrastructure.Identity;

/// <summary>
/// Application role entity extending IdentityRole
/// </summary>
public sealed class ApplicationRole : IdentityRole
{
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
    public ICollection<ApplicationRolePermission> RolePermissions { get; set; } = [];
}