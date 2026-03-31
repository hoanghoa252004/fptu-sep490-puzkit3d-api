using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Partner.PartnerProducts;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.Partner.PartnerProducts;

internal sealed class PartnerProductUpdatedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductUpdatedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public PartnerProductUpdatedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        PartnerProductUpdatedIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        var product = await _context.PartnerProductReplicas
            .FirstOrDefaultAsync(p => p.Id == @event.ProductId, cancellationToken);

        if (product == null)
        {
            return;
        }

        product.Update(
            @event.PartnerId,
            @event.Name,
            @event.ReferencePrice,
            @event.Quantity,
            @event.Description,
            @event.ThumbnailUrl,
            @event.PreviewAsset,
            @event.Slug,
            @event.UpdatedAt
            );

        await _context.SaveChangesAsync(cancellationToken);
    }
}
