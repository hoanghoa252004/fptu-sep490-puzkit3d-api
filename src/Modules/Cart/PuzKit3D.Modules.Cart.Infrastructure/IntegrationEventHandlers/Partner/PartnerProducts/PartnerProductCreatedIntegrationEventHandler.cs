using PuzKit3D.Contract.Partner.PartnerProducts;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.Partner.PartnerProducts;

internal sealed class PartnerProductCreatedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductCreatedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public PartnerProductCreatedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(PartnerProductCreatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
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
