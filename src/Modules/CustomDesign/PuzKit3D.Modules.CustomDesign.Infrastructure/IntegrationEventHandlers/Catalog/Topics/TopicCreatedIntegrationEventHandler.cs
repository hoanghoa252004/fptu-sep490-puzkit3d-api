using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Topics;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;
using PuzKit3D.Modules.CustomDesign.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.CustomDesign.Infrastructure.IntegrationEventHandlers.Catalog.Topics;

internal sealed class TopicCreatedIntegrationEventHandler
    : IIntegrationEventHandler<TopicCreatedIntegrationEvent>
{
    private readonly CustomDesignDbContext _context;

    public TopicCreatedIntegrationEventHandler(CustomDesignDbContext context)
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
            @event.Description,
            @event.ParentId,
            @event.IsActive,
            @event.CreatedAt,
            @event.CreatedAt);

        _context.TopicReplicas.Add(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
