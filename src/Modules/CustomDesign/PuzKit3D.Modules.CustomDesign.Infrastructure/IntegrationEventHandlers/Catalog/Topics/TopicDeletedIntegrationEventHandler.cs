using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Catalog.Topics;
using PuzKit3D.Modules.CustomDesign.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.CustomDesign.Infrastructure.IntegrationEventHandlers.Catalog.Topics;

internal sealed class TopicDeletedIntegrationEventHandler
    : IIntegrationEventHandler<TopicDeletedIntegrationEvent>
{
    private readonly CustomDesignDbContext _context;

    public TopicDeletedIntegrationEventHandler(CustomDesignDbContext context)
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

        _context.TopicReplicas.Remove(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
