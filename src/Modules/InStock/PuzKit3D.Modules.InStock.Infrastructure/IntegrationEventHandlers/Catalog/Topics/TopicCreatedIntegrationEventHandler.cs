using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Topics;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.Catalog.Topics;

internal sealed class TopicCreatedIntegrationEventHandler
    : IIntegrationEventHandler<TopicCreatedIntegrationEvent>
{
    private readonly InStockDbContext _context;

    public TopicCreatedIntegrationEventHandler(InStockDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        TopicCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var existingReplica = await _context.TopicReplicas
            .FirstOrDefaultAsync(t => t.Id == @event.TopicId, cancellationToken);

        if (existingReplica is not null)
        {
            // Already exists, skip
            return;
        }

        var replica = TopicReplica.Create(
            @event.TopicId,
            @event.Name,
            @event.Slug,
            @event.ParentId,
            @event.FactorPercentage,
            @event.Description,
            @event.IsActive,
            @event.CreatedAt,
            @event.CreatedAt);

        _context.TopicReplicas.Add(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
