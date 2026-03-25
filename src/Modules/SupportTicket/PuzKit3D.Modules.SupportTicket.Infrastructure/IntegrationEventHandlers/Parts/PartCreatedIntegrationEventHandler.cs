using PuzKit3D.Contract.InStock.Part;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.PartReplicas;
using PuzKit3D.Modules.SupportTicket.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.SupportTicket.Infrastructure.IntegrationEventHandlers.Parts;

internal class PartCreatedIntegrationEventHandler : IIntegrationEventHandler<PartCreatedIntegrationEvent>
{
    private readonly SupportTicketDbContext _dbContext;
    public PartCreatedIntegrationEventHandler(
        SupportTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task HandleAsync(PartCreatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var partReplica = PartReplica.Create(
            @event.PartId,
            @event.Name,
            @event.PartType,
            @event.Code,
            @event.Quantity,
            @event.InstockProductId);

        _dbContext.PartReplicas.Add(partReplica);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
