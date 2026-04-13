using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Delivery.Application.Repositories;

public interface IDriveReplicaRepository
{
    Task<DriveReplica?> GetByIdAsync(Guid driveId, CancellationToken cancellationToken = default);
    Task<Result> AddAsync(DriveReplica replica, CancellationToken cancellationToken = default);
}

