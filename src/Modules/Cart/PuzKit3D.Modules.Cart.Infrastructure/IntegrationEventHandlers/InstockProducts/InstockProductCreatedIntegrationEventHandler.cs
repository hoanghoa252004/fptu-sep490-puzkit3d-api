using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockProducts;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockProducts;

internal sealed class InstockProductCreatedIntegrationEventHandler
    : IIntegrationEventHandler<InstockProductCreatedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockProductCreatedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockProductCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var existingProduct = await _context.InStockProductReplicas
            .FirstOrDefaultAsync(p => p.Id == @event.ProductId, cancellationToken);

        if (existingProduct is not null)
        {
            // Already exists, skip
            return;
        }

        var replica = InStockProductReplica.Create(
            @event.ProductId,
            @event.Code,
            @event.Name,
            @event.Description, 
            @event.DifficultLevel,
            @event.EstimatedBuildTime,
            @event.TotalPieceCount,
            @event.ThumbnailUrl,
            @event.Slug,
            @event.PreviewAsset,
            @event.TopicId,
            @event.AssemblyMethodId,
            @event.MaterialId,
            @event.IsActive,
            @event.CreatedAt,
            @event.CreatedAt);

        _context.InStockProductReplicas.Add(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
