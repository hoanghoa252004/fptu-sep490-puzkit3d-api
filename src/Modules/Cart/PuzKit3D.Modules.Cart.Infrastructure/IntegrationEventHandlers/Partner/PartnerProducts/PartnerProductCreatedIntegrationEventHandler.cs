using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Partner.PartnerProducts;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.Partner.PartnerProducts;

internal sealed class PartnerProductCreatedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductCreatedIntegrationEvent>
{
    private readonly ICartDbContext _context;

    public PartnerProductCreatedIntegrationEventHandler(ICartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(PartnerProductCreatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var existingProduct = await _context.PartnerProductReplicas
            .FirstOrDefaultAsync(p => p.Id == @event.ProductId, cancellationToken);

        if (existingProduct != null)
        {
            return;
        }

        var product = PartnerProductReplica.Create(
            @event.ProductId,
            @event.PartnerId,
            @event.Name,
            @event.ReferencePrice,
            @event.Quantity,
            @event.Description,
            @event.ThumbnailUrl,
            @event.PreviewAsset,
            @event.Slug,
            @event.IsActive,
            @event.CreatedAt);

        _context.PartnerProductReplicas.Add(product);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
