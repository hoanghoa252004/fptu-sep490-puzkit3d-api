using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PuzKit3D.SharedKernel.Application.Clock;
using PuzKit3D.SharedKernel.Application.Data;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Infrastructure.Data;

/// <summary>
/// Base DbContext with domain events dispatch and audit fields
/// </summary>
public abstract class BaseDbContext : DbContext
{

    protected BaseDbContext(
        DbContextOptions options) : base(options)
    {
    }
}
