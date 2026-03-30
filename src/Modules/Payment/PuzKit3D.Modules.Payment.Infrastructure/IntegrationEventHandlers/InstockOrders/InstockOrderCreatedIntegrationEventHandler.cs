using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.Payment.Application.UnitOfWork;
using PuzKit3D.Modules.Payment.Domain.Entities.OrderReplicas;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.Modules.Payment.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

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
        // Get PaymentConfig from database, use default values if not found
        var paymentConfig = await _dbContext.PaymentConfigs.FirstOrDefaultAsync(cancellationToken);
        var onlinePaymentExpiredInDays = paymentConfig?.OnlinePaymentExpiredInDays ?? 1;

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

        // If grand total amount is 0, create a COIN payment with Amount = 0, marked as Paid immediately
        if (@event.GrandTotalAmount == 0)
        {
            var coinPaymentResult = Domain.Entities.Payments.Payment.Create(
                referenceOrderId: @event.OrderId,
                referenceOrderType: OrderType.Instock,
                amount: @event.UsedCoinAmount,
                paymentMethod: "COIN");

            if (coinPaymentResult.IsSuccess)
            {
                var coinPayment = coinPaymentResult.Value;
                
                // Mark payment as paid immediately with current timestamp
                var now = DateTime.UtcNow;
                var updateStatusResult = coinPayment.UpdateStatus(PaymentStatus.Paid, now);
                
                if (updateStatusResult.IsSuccess)
                {
                    await _dbContext.Payments.AddAsync(coinPayment, cancellationToken);
                }
            }
        }
        else
        {
            // Normal payment creation for non-zero amounts
            var paymentResult = Domain.Entities.Payments.Payment.Create(
                referenceOrderId: @event.OrderId,
                referenceOrderType: OrderType.Instock,
                amount: @event.GrandTotalAmount,
                paymentMethod: @event.PaymentMethod,
                expirationDays: onlinePaymentExpiredInDays);

            if (paymentResult.IsFailure)
            {
                throw new PuzKit3DException($"Failed to create payment for order {orderReplica.Id}: {paymentResult.Error.Message}");
            }

            await _dbContext.Payments.AddAsync(paymentResult.Value, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

