using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Application.Data;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.Payments;

internal sealed class OrderExpiredToDoPaymentIntegrationEventHandler : IIntegrationEventHandler<OrderExpiredToDoPaymentIntegrationEvent>
{
    private readonly IInstockOrderRepository _instockOrderRepository;
    private readonly IInStockUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public OrderExpiredToDoPaymentIntegrationEventHandler(
        IInstockOrderRepository instockOrderRepository,
        IInStockUnitOfWork unitOfWork,
        IEventBus eventBus)
    {
        _instockOrderRepository = instockOrderRepository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task HandleAsync(
        OrderExpiredToDoPaymentIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        // Get the order by ID
        var order = await _instockOrderRepository.GetByIdAsync(
            InstockOrderId.From(@event.OrderId),
            cancellationToken);

        if (order == null)
            return;

        // Update order status to Expired
        var markAsExpiredResult = order.MarkAsExpired();
        
        if (markAsExpiredResult.IsFailure)
            return;

        // Save changes
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Publish InstockOrderStatusChangedIntegrationEvent
        var statusChangedEvent = new InstockOrderStatusChangedIntegrationEvent(
            Guid.NewGuid(),
            DateTime.UtcNow,
            order.Id.Value,
            order.Code,
            order.CustomerId,
            InstockOrderStatus.Expired.ToString(),
            DateTime.UtcNow);

        await _eventBus.PublishAsync(statusChangedEvent, cancellationToken);
    }
}

