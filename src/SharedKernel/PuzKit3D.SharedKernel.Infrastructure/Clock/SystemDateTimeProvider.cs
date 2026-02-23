using PuzKit3D.SharedKernel.Application.Clock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Infrastructure.Clock;

/// <summary>
/// System implementation of IDateTimeProvider using actual system time
/// </summary>
public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime Now => DateTime.Now;

    public DateOnly Today => DateOnly.FromDateTime(DateTime.Today);

    public TimeOnly CurrentTime => TimeOnly.FromDateTime(DateTime.Now);
}