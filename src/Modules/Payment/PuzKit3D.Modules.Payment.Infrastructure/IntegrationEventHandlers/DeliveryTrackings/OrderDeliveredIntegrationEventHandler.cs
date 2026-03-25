using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.Delivery;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.Modules.Payment.Application.UnitOfWork;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.Modules.Payment.Persistence;
using PuzKit3D.SharedKernel.Application.Data;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Payment.Infrastructure.IntegrationEventHandlers.DeliveryTrackings;

public sealed class OrderDeliveredIntegrationEventHandler : IIntegrationEventHandler<OrderDeliveredIntegrationEvent>
{

    private readonly PaymentDbContext _dbContext;
    private readonly IPaymentUnitOfWork _unitOfWork;

    public OrderDeliveredIntegrationEventHandler(
        PaymentDbContext dbContext,
        IPaymentUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }


    public async Task HandleAsync(
        OrderDeliveredIntegrationEvent @event,
        CancellationToken cancellationToken)
    {
        // Find payment by ReferenceOrderId (OrderId)
        var payment = await _dbContext.Payments
            .FirstOrDefaultAsync(p => p.ReferenceOrderId == @event.OrderId, cancellationToken);

        if (payment == null)
            return;

        // Only update payment status if payment method is COD (Cash On Delivery)
        if (!payment.PaymentMethod.Equals("COD", StringComparison.OrdinalIgnoreCase))
            return;

        // Update payment status to Paid
        var updateResult = payment.UpdateStatus(PaymentStatus.Paid, DateTime.UtcNow);

        if (updateResult.IsFailure)
            return;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}


