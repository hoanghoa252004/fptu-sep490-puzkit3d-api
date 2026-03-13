using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.Payment.Application.UnitOfWork;
using PuzKit3D.Modules.Payment.Domain.Entities.OrderReplicas;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.Modules.Payment.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

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
        var orderReplica = OrderReplica.Create(
            @event.OrderId,
            OrderType.Instock,
            @event.Code,
            @event.CustomerId,
            @event.GrandTotalAmount,
            @event.Status,
            @event.PaymentMethod,
            @event.IsPaid,
            @event.PaidAt,
            @event.CreatedAt,
            @event.CreatedAt);

        await _dbContext.OrderReplicas.AddAsync(orderReplica, cancellationToken);

        var paymentResult = Domain.Entities.Payments.Payment.Create(
            referenceOrderId: @event.OrderId,
            referenceOrderType: OrderType.Instock,
            amount: @event.GrandTotalAmount);

        if (paymentResult.IsFailure)
        {
            throw new InvalidOperationException($"Failed to create payment for order {orderReplica.Id}: {paymentResult.Error.Message}");
        }

        await _dbContext.Payments.AddAsync(paymentResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
