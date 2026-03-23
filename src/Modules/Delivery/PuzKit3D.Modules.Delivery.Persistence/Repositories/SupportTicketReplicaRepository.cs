using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Persistence.Repositories;

internal sealed class SupportTicketReplicaRepository : ISupportTicketReplicaRepository
{
    private readonly DeliveryDbContext _context;

    public SupportTicketReplicaRepository(DeliveryDbContext context)
    {
        _context = context;
    }

    public async Task<ResultT<SupportTicketReplica>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var ticket = await _context.SupportTicketReplicas
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (ticket is null)
        {
            return Result.Failure<SupportTicketReplica>(
                Error.NotFound("SupportTicket.NotFound", $"Support ticket with ID {id} not found"));
        }

        return Result.Success(ticket);
    }

    public async Task<ResultT<List<SupportTicketDetailReplica>>> GetDetailsByTicketIdAsync(Guid ticketId, CancellationToken cancellationToken = default)
    {
        var details = await _context.SupportTicketDetailReplicas
            .Where(d => d.SupportTicketId == ticketId)
            .ToListAsync(cancellationToken);

        if (!details.Any())
        {
            return Result.Failure<List<SupportTicketDetailReplica>>(
                Error.NotFound("SupportTicketDetails.NotFound", $"No details found for support ticket {ticketId}"));
        }

        return Result.Success(details);
    }
}
