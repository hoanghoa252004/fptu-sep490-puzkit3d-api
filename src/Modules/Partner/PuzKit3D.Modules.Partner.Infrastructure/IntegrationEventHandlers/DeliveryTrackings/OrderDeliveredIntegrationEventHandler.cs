using PuzKit3D.Contract.Delivery;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrderConfigs;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;
using PuzKit3D.SharedKernel.Application.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Partner.Infrastructure.IntegrationEventHandlers.DeliveryTrackings;

public sealed class OrderDeliveredIntegrationEventHandler : IIntegrationEventHandler<OrderDeliveredIntegrationEvent>
{
    private readonly IPartnerProductOrderRepository _repository;
    private readonly IPartnerUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public OrderDeliveredIntegrationEventHandler(
        IPartnerProductOrderRepository repository,
        IPartnerUnitOfWork unitOfWork,
        IEventBus eventBus)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task HandleAsync(
        OrderDeliveredIntegrationEvent @event,
        CancellationToken cancellationToken)
    {
        var orderId = PartnerProductOrderId.From(@event.OrderId);
        var order = await _repository.GetByIdAsync(orderId, cancellationToken);

        if (order == null)
            return;

        //var instockOrderConfig = await _instockOrderConfigRepository.GetFirstAsync(cancellationToken);
        //var orderMustCompleteInDays = instockOrderConfig?.OrderMustCompleteInDays ?? 7;
        var orderMustCompleteInDays = 7;

        // Set mustCompleteBefore based on config
        var mustCompleteBefore = DateTime.UtcNow.AddDays(orderMustCompleteInDays);
        var setMustCompleteResult = order.SetMustCompleteBefore(mustCompleteBefore);
        if (setMustCompleteResult.IsFailure)
            return;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
