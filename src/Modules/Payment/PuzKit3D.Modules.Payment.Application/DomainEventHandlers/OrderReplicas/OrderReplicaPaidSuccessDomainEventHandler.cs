using MediatR;
using Microsoft.Extensions.Logging;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.Payment.Domain.Entities.OrderReplicas;
using PuzKit3D.Modules.Payment.Domain.Entities.OrderReplicas.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Payment.Application.DomainEventHandlers.OrderReplicas;

internal sealed class OrderReplicaPaidSuccessDomainEventHandler
    : INotificationHandler<OrderReplicaPaidSuccessDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<OrderReplicaPaidSuccessDomainEventHandler> _logger;

    public OrderReplicaPaidSuccessDomainEventHandler(
        IEventBus eventBus,
        ILogger<OrderReplicaPaidSuccessDomainEventHandler> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task Handle(
        OrderReplicaPaidSuccessDomainEvent @event,
        CancellationToken cancellationToken)
    {
        try
        {

            // Only publish event if it's an InStock order
            if (@event.OrderType == OrderType.Instock)
            {
                await _eventBus.PublishAsync(new InstockOrderPaidSuccessIntegrationEvent(
                Id: Guid.NewGuid(),
                OccurredOn: DateTime.UtcNow,
                OrderId: @event.OrderReplicaId,
                Code: @event.Code,
                CustomerId: @event.CustomerId,
                GrandTotalAmount: @event.Amount,
                PaidAt: @event.PaidAt), cancellationToken);
            }
            else if (@event.OrderType == OrderType.Partner)
            {
                await _eventBus.PublishAsync(new PartnerProductOrderPaidSuccessIntegrationEvent(
                Id: Guid.NewGuid(),
                OccurredOn: DateTime.UtcNow,
                OrderId: @event.OrderReplicaId,
                Code: @event.Code,
                CustomerId: @event.CustomerId,
                GrandTotalAmount: @event.Amount,
                PaidAt: @event.PaidAt), cancellationToken);
            }

            _logger.LogInformation(
                "Published InstockOrderPaidIntegrationEvent for OrderReplica. OrderId: {OrderId}, PaidAt: {PaidAt}",
                @event.OrderReplicaId, @event.PaidAt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling OrderReplicaPaidDomainEvent for OrderId: {OrderId}",
                @event.OrderReplicaId);
            throw;
        }
    }
}
