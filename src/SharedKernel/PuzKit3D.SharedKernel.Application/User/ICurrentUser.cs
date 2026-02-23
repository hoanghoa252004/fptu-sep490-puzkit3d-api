using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.User;

public interface ICurrentUser
{
    /// <summary>
    /// Gets the current user's ID
    /// </summary>
    string? UserId { get; }

    /// <summary>
    /// Gets the current user's email
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Checks if the user is authenticated
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Gets all roles of the current user
    /// </summary>
    IEnumerable<string> Roles { get; }

    /// <summary>
    /// Gets all claims of the current user
    /// </summary>
    IDictionary<string, string> Claims { get; }

    /// <summary>
    /// Checks if the current user has a specific role
    /// </summary>
    bool IsInRole(string role);

    /// <summary>
    /// Checks if the current user has a specific claim
    /// </summary>
    bool HasClaim(string claimType, string claimValue);
}
