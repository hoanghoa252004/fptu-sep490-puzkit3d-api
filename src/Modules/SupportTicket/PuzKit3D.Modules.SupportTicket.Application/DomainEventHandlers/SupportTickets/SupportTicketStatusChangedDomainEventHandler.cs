using MediatR;
using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.DomainEvents;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.SupportTicket.Application.DomainEventHandlers.SupportTickets;

internal sealed class SupportTicketStatusChangedDomainEventHandler
    : INotificationHandler<SupportTicketStatusChangedDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly IOrderReplicaRepository _orderReplicaRepository;
    private readonly ISupportTicketDetailRepository _supportTicketDetailRepository;

    public SupportTicketStatusChangedDomainEventHandler(
        IEventBus eventBus,
        IOrderReplicaRepository orderReplicaRepository,
        ISupportTicketDetailRepository supportTicketDetailRepository)
    {
        _eventBus = eventBus;
        _orderReplicaRepository = orderReplicaRepository;
        _supportTicketDetailRepository = supportTicketDetailRepository;
    }

    public async Task Handle(
        SupportTicketStatusChangedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var integrationEvent = new SupportTicketStatusChangedIntegrationEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            notification.SupportTicketId,
            notification.Status,
            notification.UpdatedAt);

        await _eventBus.PublishAsync(integrationEvent, cancellationToken);

        // Handle Exchange type support tickets when status changes to Processing
        if (notification.Type == SupportTicketType.Exchange.ToString() && 
            notification.Status == SupportTicketStatus.Processing.ToString())
        {
            var orderResult = await _orderReplicaRepository.GetByIdAsync(notification.OrderId, cancellationToken);
            if (orderResult.IsSuccess)
            {
                var order = orderResult.Value;

                // Only process if order type is Instock
                if (order.Type.Equals("Instock", StringComparison.OrdinalIgnoreCase))
                {
                    // Get support ticket details
                    var detailsResult = await _supportTicketDetailRepository.GetBySupportTicketIdAsync(
                        notification.SupportTicketId, 
                        cancellationToken);

                    if (detailsResult.IsSuccess && detailsResult.Value.Any())
                    {
                        var items = detailsResult.Value
                            .Select(d => new ExchangeItem(d.OrderItemId, d.Quantity))
                            .ToList();

                        var exchangeEvent = new SupportTicketExchangeProcessingIntegrationEvent(
                            Guid.NewGuid(),
                            DateTime.UtcNow,
                            notification.SupportTicketId,
                            notification.OrderId,
                            items);

                        await _eventBus.PublishAsync(exchangeEvent, cancellationToken);
                    }
                }
            }
        }

        // Handle ReplaceDrive type support tickets when status changes to Processing
        if (notification.Type == SupportTicketType.ReplaceDrive.ToString() && 
            notification.Status == SupportTicketStatus.Processing.ToString())
        {
            // Get support ticket details
            var detailsResult = await _supportTicketDetailRepository.GetBySupportTicketIdAsync(
                notification.SupportTicketId, 
                cancellationToken);

            if (detailsResult.IsSuccess && detailsResult.Value.Any())
            {
                var items = detailsResult.Value
                    .Where(d => d.DriveId.HasValue)
                    .Select(d => new ReplaceDriveItem(d.DriveId!.Value, d.Quantity))
                    .ToList();

                if (items.Any())
                {
                    var replaceDriveEvent = new SupportTicketReplaceDriveProcessingIntegrationEvent(
                        Guid.NewGuid(),
                        DateTime.UtcNow,
                        notification.SupportTicketId,
                        notification.OrderId,
                        items);

                    await _eventBus.PublishAsync(replaceDriveEvent, cancellationToken);
                }
            }
        }

        // Handle Return type support tickets when status changes to Resolved
        if (notification.Type == SupportTicketType.Return.ToString() && 
            notification.Status == SupportTicketStatus.Resolved.ToString())
        {
            var orderResult = await _orderReplicaRepository.GetByIdAsync(notification.OrderId, cancellationToken);
            if (orderResult.IsSuccess)
            {
                var order = orderResult.Value;
                await _eventBus.PublishAsync(new SupportTicketTypeResendResolvedIntegrationEvent(
                    Guid.NewGuid(),
                    DateTime.UtcNow, 
                    order.CustomerId, 
                    order.Id, 
                    order.GrandTotalAmount
                ), cancellationToken);
            }
        }
    }
}

