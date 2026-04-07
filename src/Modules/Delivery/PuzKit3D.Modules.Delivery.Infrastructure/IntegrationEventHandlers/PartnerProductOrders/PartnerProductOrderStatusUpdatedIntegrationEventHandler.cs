using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.Delivery.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.PartnerProductOrders;

internal sealed class PartnerProductOrderStatusUpdatedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductOrderStatusUpdatedIntegrationEvent>
{
    private readonly DeliveryDbContext _dbContext;

    public PartnerProductOrderStatusUpdatedIntegrationEventHandler(DeliveryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        PartnerProductOrderStatusUpdatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var orderReplica = await _dbContext.OrderReplicas.FindAsync(
            new object[] { @event.OrderId }, cancellationToken);

        if (orderReplica is null)
        {
            return;
        }

        orderReplica.Update(@event.NewStatus);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
