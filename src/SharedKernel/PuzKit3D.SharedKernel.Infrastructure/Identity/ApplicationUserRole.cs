using Microsoft.AspNetCore.Identity;

namespace PuzKit3D.SharedKernel.Infrastructure.Identity;

/// <summary>
/// Many-to-many relationship between Users and Roles
/// </summary>
public sealed class ApplicationUserRole : IdentityUserRole<string>
{
    public ApplicationUser User { get; set; } = null!;
    public ApplicationRole Role { get; set; } = null!;
}
