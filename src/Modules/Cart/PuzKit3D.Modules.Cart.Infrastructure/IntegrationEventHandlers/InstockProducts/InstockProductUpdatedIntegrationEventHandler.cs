using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockProducts;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockProducts;

internal sealed class InstockProductUpdatedIntegrationEventHandler
    : IIntegrationEventHandler<InstockProductUpdatedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockProductUpdatedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        InstockProductUpdatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var product = await _context.InStockProductReplicas
            .FirstOrDefaultAsync(p => p.Id == @event.ProductId, cancellationToken);

        if (product is null)
        {
            // Product not found, skip
            return;
        }

        product.Update(
            @event.Code,
            @event.Name,
            @event.DifficultLevel,
            @event.EstimatedBuildTime,
            @event.ThumbnailUrl,
            @event.Slug,
            @event.PreviewAsset,
            @event.TopicId,
            @event.AssemblyMethodId,
            @event.MaterialId,
            @event.Description,
            @event.IsActive,
            @event.UpdatedAt);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
