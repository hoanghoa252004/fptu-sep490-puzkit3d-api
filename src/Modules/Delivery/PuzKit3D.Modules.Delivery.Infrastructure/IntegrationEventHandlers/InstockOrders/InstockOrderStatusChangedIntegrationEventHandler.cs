using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.Delivery.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.InstockOrders;

internal sealed class InstockOrderStatusChangedIntegrationEventHandler
    : IIntegrationEventHandler<InstockOrderStatusChangedIntegrationEvent>
{
    private readonly DeliveryDbContext _dbContext;


    public InstockOrderStatusChangedIntegrationEventHandler(
        DeliveryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        InstockOrderStatusChangedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var orderReplica = await _dbContext.OrderReplicas.FindAsync(
            new object[] { @event.OrderId }, cancellationToken);

        if (orderReplica is null)
        {
            return;
        }

        orderReplica.Update(
            @event.NewStatus);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

