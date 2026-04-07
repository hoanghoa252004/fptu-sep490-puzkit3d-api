using PuzKit3D.Contract.Delivery;
using PuzKit3D.Contract.Wallet;
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

public sealed class OrderReturnedIntegrationEventHandler : IIntegrationEventHandler<OrderReturnedIntegrationEvent>
{
    private readonly IPartnerProductOrderRepository _repository;
    private readonly IPartnerProductRepository _partnerProductRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;

    public OrderReturnedIntegrationEventHandler(
        IPartnerProductOrderRepository repository,
        IPartnerUnitOfWork unitOfWork,
        IPartnerProductRepository partnerProductRepository,
        IEventBus eventBus)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _partnerProductRepository = partnerProductRepository;
        _eventBus = eventBus;
    }

    public async Task HandleAsync(
        OrderReturnedIntegrationEvent @event,
        CancellationToken cancellationToken)
    {
        var orderId = PartnerProductOrderId.From(@event.OrderId);
        var order = await _repository.GetByIdWithDetailsAsync(orderId, cancellationToken);

        if (order == null)
            return;

        // Update order status to Returned
        var markAsReturnedResult = order.MarkAsReturned();

        if (markAsReturnedResult.IsFailure)
            return;

        // Increase stock quantity for each returned product
        foreach (var item in order.Details)
        {
            var partnerProduct = await _partnerProductRepository.GetByIdAsync(item.PartnerProductId, cancellationToken);
            if (partnerProduct == null)
                continue;
            partnerProduct.IncreaseQuantity(item.Quantity);
        }

        // Publish refund coin event to Wallet module
        await _eventBus.PublishAsync(
            new OrderReturnRefundCoinIntegrationEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                @event.OrderId,
                order.GrandTotalAmount,
                order.UsedCoinAmount,
                order.PaymentMethod,
                order.ShippingFee,
                order.CustomerId),
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}