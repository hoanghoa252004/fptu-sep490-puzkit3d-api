using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Topics;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.Catalog.Topics;

internal sealed class TopicUpdatedIntegrationEventHandler
    : IIntegrationEventHandler<TopicUpdatedIntegrationEvent>
{
    private readonly InStockDbContext _context;

    public TopicUpdatedIntegrationEventHandler(InStockDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        TopicUpdatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.TopicReplicas
            .FirstOrDefaultAsync(t => t.Id == @event.TopicId, cancellationToken);

        if (replica is null)
        {
            // Replica doesn't exist, skip
            return;
        }

        replica.Update(
            @event.Name,
            @event.Slug,
            @event.Description,
            @event.ParentId,
            replica.IsActive, // Keep existing IsActive value
            @event.UpdatedAt);

        _context.TopicReplicas.Update(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
