using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Clock;

/// <summary>
/// Abstraction for getting current date and time (for testability)
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Gets the current UTC date and time
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Gets the current local date and time
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    /// Gets the current date only
    /// </summary>
    DateOnly Today { get; }

    /// <summary>
    /// Gets the current time only
    /// </summary>
    TimeOnly CurrentTime { get; }
}