using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Authentication;

/// <summary>
/// Result of successful authentication
/// </summary>
public sealed record AuthenticationResult(
    string UserId,
    string Email,
    string Token,
    string RefreshToken,
    DateTime ExpiresAt
);
