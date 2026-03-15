using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Topics;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Exceptions;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.Catalog.Topics;

internal sealed class TopicDeletedIntegrationEventHandler
    : IIntegrationEventHandler<TopicDeletedIntegrationEvent>
{
    private readonly InStockDbContext _context;

    public TopicDeletedIntegrationEventHandler(InStockDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        TopicDeletedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = await _context.TopicReplicas
            .FirstOrDefaultAsync(t => t.Id == @event.TopicId, cancellationToken);

        if (replica is null)
        {
            // Replica doesn't exist, skip
            return;
        }

        // Check if any InstockProduct is using this Topic
        var hasProducts = await _context.InstockProducts
            .AnyAsync(p => p.TopicId == @event.TopicId, cancellationToken);

        if (hasProducts)
        {
            throw new PuzKit3DException("This topic can be delete because there is one or more products belongs");
        }

        _context.TopicReplicas.Remove(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

