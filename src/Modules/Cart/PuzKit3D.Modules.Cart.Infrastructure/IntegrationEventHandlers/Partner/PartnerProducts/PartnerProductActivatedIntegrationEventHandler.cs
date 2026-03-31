using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Partner.PartnerProducts;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.Partner.PartnerProducts;

internal sealed class PartnerProductActivatedIntegrationEventHandler
    :IIntegrationEventHandler<PartnerProductActivatedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public PartnerProductActivatedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(
        PartnerProductActivatedIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        var product = await _context.PartnerProductReplicas
            .FirstOrDefaultAsync(p => p.Id == @event.ProductId);

        if (product is null)
        {
            return;
        }

        product.Activate();
        _context.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
