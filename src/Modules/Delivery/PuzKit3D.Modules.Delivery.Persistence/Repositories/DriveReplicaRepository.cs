using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Delivery.Persistence.Repositories;

internal sealed class DriveReplicaRepository : IDriveReplicaRepository
{
    private readonly DeliveryDbContext _dbContext;

    public DriveReplicaRepository(DeliveryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DriveReplica?> GetByIdAsync(Guid driveId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.DriveReplicas
            .FirstOrDefaultAsync(p => p.Id == driveId, cancellationToken);
    }

    public async Task<Result> AddAsync(DriveReplica replica, CancellationToken cancellationToken = default)
    {
        await _dbContext.DriveReplicas.AddAsync(replica, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
