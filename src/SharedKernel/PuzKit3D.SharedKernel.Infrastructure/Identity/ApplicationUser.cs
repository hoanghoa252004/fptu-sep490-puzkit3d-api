using Microsoft.AspNetCore.Identity;

namespace PuzKit3D.SharedKernel.Infrastructure.Identity;

/// <summary>
/// Application user entity extending IdentityUser
/// </summary>
public sealed class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    // Navigation properties
    public ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
    public ICollection<ApplicationUserPermission> UserPermissions { get; set; } = [];
}
