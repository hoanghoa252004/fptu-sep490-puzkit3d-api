using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Domain.Entities;
using PuzKit3D.Modules.SupportTicket.Persistence;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Persistence.Repositories;

internal sealed class DriveReplicaRepository : IDriveReplicaRepository
{
    private readonly SupportTicketDbContext _dbContext;

    public DriveReplicaRepository(SupportTicketDbContext dbContext)
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
