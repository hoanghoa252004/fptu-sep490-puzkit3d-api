using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.Modules.Delivery.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.InstockOrders;

internal sealed class InstockOrderCreatedIntegrationEventHandler
    : IIntegrationEventHandler<InstockOrderCreatedIntegrationEvent>
{
    private readonly DeliveryDbContext _dbContext;

    public InstockOrderCreatedIntegrationEventHandler(
        DeliveryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        InstockOrderCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = OrderReplica.Create(
            @event.OrderId,
            "Instock",
            @event.CustomerId,
            @event.Code,
            @event.Status,
            @event.GrandTotalAmount);

        await _dbContext.OrderReplicas.AddAsync(replica, cancellationToken);

        foreach (var orderDetail in @event.OrderDetails)
        {

            var orderDetailReplica = OrderDetailReplica.Create(
                orderDetail.OrderDetailId,
                @event.OrderId,
                orderDetail.ProductId,
                orderDetail.VariantId,
                orderDetail.Quantity,
                orderDetail.ProductName,
                orderDetail.VariantName);

            await _dbContext.OrderDetailReplicas.AddAsync(orderDetailReplica, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
