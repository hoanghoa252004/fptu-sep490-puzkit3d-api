using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Infrastructure.Identity;

/// <summary>
/// User-specific permissions
/// </summary>
public sealed class ApplicationUserPermission
{
    public string UserId { get; set; } = null!;
    public string Permission { get; set; } = null!;
    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    public string? GrantedBy { get; set; }

    public ApplicationUser User { get; set; } = null!;
}
