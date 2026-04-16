using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.Modules.Delivery.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.PartnerProductOrders;

internal sealed class PartnerProductOrderCreatedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductOrderCreatedIntegrationEvent>
{
    private readonly DeliveryDbContext _dbContext;

    public PartnerProductOrderCreatedIntegrationEventHandler(
        DeliveryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        PartnerProductOrderCreatedIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    
    {
        var existed = await _dbContext.OrderReplicas
            .FirstOrDefaultAsync(x => x.Id == @event.OrderId, cancellationToken);

        if (existed != null)
        {
            return;
        }

        var orderReplica = OrderReplica.Create(
            @event.OrderId,
            "Partner",
            @event.CustomerId,
            @event.Code,
            @event.Status,
            @event.GrandTotalAmount);

        await _dbContext.OrderReplicas.AddAsync(orderReplica, cancellationToken);

        foreach (var orderDetail in @event.Details)
        {
            var orderDetailReplica = OrderDetailReplica.Create(
                orderDetail.OrderDetailId,
                @event.OrderId,
                orderDetail.PartnerProductId,
                null,
                orderDetail.Quantity,
                orderDetail.ProductName,
                null);
            await _dbContext.OrderDetailReplicas.AddAsync(orderDetailReplica, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
