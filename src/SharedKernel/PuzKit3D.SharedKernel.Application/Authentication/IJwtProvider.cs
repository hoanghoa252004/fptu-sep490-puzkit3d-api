using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Authentication;

/// <summary>
/// Service for generating JWT tokens
/// </summary>
public interface IJwtProvider
{
    /// <summary>
    /// Generates a JWT token for the authenticated user
    /// </summary>
    Task<ResultT<string>> GenerateTokenAsync(
        string userId,
        string email,
        CancellationToken cancellationToken = default);
}
