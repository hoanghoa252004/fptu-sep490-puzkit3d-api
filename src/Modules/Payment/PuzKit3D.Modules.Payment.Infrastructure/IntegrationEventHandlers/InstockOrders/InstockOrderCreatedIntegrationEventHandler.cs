using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.Payment.Application.UnitOfWork;
using PuzKit3D.Modules.Payment.Domain.Entities.OrderReplicas;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.Modules.Payment.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Exceptions;

namespace PuzKit3D.Modules.Payment.Infrastructure.IntegrationEventHandlers.InstockOrders;

internal sealed class InstockOrderCreatedIntegrationEventHandler
    : IIntegrationEventHandler<InstockOrderCreatedIntegrationEvent>
{
    private readonly PaymentDbContext _dbContext;
    private readonly IPaymentUnitOfWork _unitOfWork;

    public InstockOrderCreatedIntegrationEventHandler(
        PaymentDbContext dbContext,
        IPaymentUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        InstockOrderCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var status = ConvertStatusToOrderReplicaStatus(@event.Status, @event.IsPaid);
        
        var orderReplica = OrderReplica.Create(
            @event.OrderId,
            OrderType.Instock,
            @event.Code,
            @event.CustomerId,
            @event.GrandTotalAmount,
            status,
            @event.PaymentMethod,
            @event.IsPaid,
            @event.PaidAt,
            @event.CreatedAt,
            @event.CreatedAt);

        await _dbContext.OrderReplicas.AddAsync(orderReplica, cancellationToken);

        var paymentResult = Domain.Entities.Payments.Payment.Create(
            referenceOrderId: @event.OrderId,
            referenceOrderType: OrderType.Instock,
            amount: @event.GrandTotalAmount,
            paymentMethod: @event.PaymentMethod);

        if (paymentResult.IsFailure)
        {
            throw new PuzKit3DException($"Failed to create payment for order {orderReplica.Id}: {paymentResult.Error.Message}");
        }

        await _dbContext.Payments.AddAsync(paymentResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static OrderReplicaStatus ConvertStatusToOrderReplicaStatus(string inStockStatus, bool isPaid)
    {
        return inStockStatus switch
        {
            "Paid" => OrderReplicaStatus.Paid,
            "Pending" => isPaid ? OrderReplicaStatus.Paid : OrderReplicaStatus.Pending,
            "Waiting" => OrderReplicaStatus.Pending,  // COD orders start as Waiting in InStock but Pending in Payment
            "Expired" => OrderReplicaStatus.NotPaid,
            "Cancelled" => OrderReplicaStatus.NotPaid,
            _ => OrderReplicaStatus.Pending
        };
    }
}
