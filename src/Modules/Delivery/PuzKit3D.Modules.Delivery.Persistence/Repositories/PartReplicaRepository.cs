using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Persistence.Repositories;

internal sealed class PartReplicaRepository : IPartReplicaRepository
{
    private readonly DeliveryDbContext _context;

    public PartReplicaRepository(DeliveryDbContext context)
    {
        _context = context;
    }

    public async Task<ResultT<PartReplica>> GetByIdAsync(Guid partId, CancellationToken cancellationToken = default)
    {
        // Get part information from SupportTicketDetailReplica or PartReplica
        var partReplica = await _context.PartReplicas
            .FirstOrDefaultAsync(p => p.Id == partId, cancellationToken);

        if (partReplica is null)
        {
            return Result.Failure<PartReplica>(
                Error.NotFound("Part.NotFound", $"Part with ID {partId} not found"));
        }

        return Result.Success(partReplica);
    }
}
