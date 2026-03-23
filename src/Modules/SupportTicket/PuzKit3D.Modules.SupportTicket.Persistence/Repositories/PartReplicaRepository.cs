using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.PartReplicas;
using PuzKit3D.Modules.SupportTicket.Persistence;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Persistence.Repositories;

internal sealed class PartReplicaRepository : IPartReplicaRepository
{
    private readonly SupportTicketDbContext _dbContext;

    public PartReplicaRepository(SupportTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PartReplica?> GetByIdAsync(Guid partId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.PartReplicas
            .FirstOrDefaultAsync(p => p.Id == partId, cancellationToken);
    }

    public async Task<Result> AddAsync(PartReplica replica, CancellationToken cancellationToken = default)
    {
        await _dbContext.PartReplicas.AddAsync(replica, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
