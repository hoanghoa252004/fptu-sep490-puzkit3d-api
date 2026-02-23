using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Infrastructure.Identity;

/// <summary>
/// Role-based permissions
/// </summary>
public sealed class ApplicationRolePermission
{
    public string RoleId { get; set; } = null!;
    public string Permission { get; set; } = null!;
    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;

    public ApplicationRole Role { get; set; } = null!;
}