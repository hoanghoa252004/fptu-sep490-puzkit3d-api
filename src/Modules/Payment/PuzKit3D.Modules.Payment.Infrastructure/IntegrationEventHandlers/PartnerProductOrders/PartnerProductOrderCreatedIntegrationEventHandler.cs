using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.Payment.Application.UnitOfWork;
using PuzKit3D.Modules.Payment.Domain.Entities.OrderReplicas;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.Modules.Payment.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Exceptions;

namespace PuzKit3D.Modules.Payment.Infrastructure.IntegrationEventHandlers.PartnerProductOrders;

internal sealed class PartnerProductOrderCreatedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductOrderCreatedIntegrationEvent>
{
    private readonly PaymentDbContext _dbContext;
    private readonly IPaymentUnitOfWork _unitOfWork;

    public PartnerProductOrderCreatedIntegrationEventHandler(
        PaymentDbContext dbContext, 
        IPaymentUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }
    public async Task HandleAsync(
        PartnerProductOrderCreatedIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        var orderReplica = Domain.Entities.OrderReplicas.OrderReplica.Create(
            @event.OrderId,
            OrderType.Partner,
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

        if(@event.GrandTotalAmount == 0)
        {
            var cointPaymentResult = Domain.Entities.Payments.Payment.Create(
                @event.OrderId,
                OrderType.Partner,
                @event.GrandTotalAmount,
                "COIN");

            if (cointPaymentResult.IsSuccess)
            {
                var coinPayment = cointPaymentResult.Value;

                var updatePaymentStatusResult = coinPayment.UpdateStatus(PaymentStatus.Paid, DateTime.Now);
                if (updatePaymentStatusResult.IsSuccess)
                {
                    await _dbContext.Payments.AddAsync(coinPayment, cancellationToken);
                }
            }
        }
        else
        {
            var paymentResult = Domain.Entities.Payments.Payment.Create(
                @event.OrderId,
                OrderType.Partner,
                @event.GrandTotalAmount,
                @event.PaymentMethod);

            if (paymentResult.IsFailure)
            {
                throw new PuzKit3DException($"Failed to create payment for order {orderReplica.Id}: {paymentResult.Error.Message}");
            }

            await _dbContext.Payments.AddAsync(paymentResult.Value, cancellationToken);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

}
