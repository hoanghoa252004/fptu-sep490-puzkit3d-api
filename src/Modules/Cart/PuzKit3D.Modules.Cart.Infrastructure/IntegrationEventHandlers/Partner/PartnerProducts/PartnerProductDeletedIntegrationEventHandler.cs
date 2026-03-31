using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Partner.PartnerProducts;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.Partner.PartnerProducts;

internal sealed class PartnerProductDeletedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductDeletedIntegrationEvent>
{
    private readonly ICartDbContext _context;

    public PartnerProductDeletedIntegrationEventHandler(ICartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        PartnerProductDeletedIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        var product = await _context.PartnerProductReplicas
            .FirstOrDefaultAsync(p => p.Id == @event.ProductId, cancellationToken);

        if (product is null)
        {
            return;
        }

        product.Deactivate();
        _context.PartnerProductReplicas.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
