using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.Payment.Application.UnitOfWork;
using PuzKit3D.Modules.Payment.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Payment.Infrastructure.IntegrationEventHandlers.PartnerProductOrders;

internal sealed class PartnerProductOrderStatusUpdatedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductOrderStatusUpdatedIntegrationEvent>
{
    private readonly PaymentDbContext _dbContext;
    private readonly IPaymentUnitOfWork _unitOfWork;

    public PartnerProductOrderStatusUpdatedIntegrationEventHandler(
        PaymentDbContext dbContext, 
        IPaymentUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        PartnerProductOrderStatusUpdatedIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        var orderReplica = await _dbContext.OrderReplicas.FindAsync(
            new object[] { @event.OrderId }, cancellationToken);

        if (orderReplica is null) { return; }

        orderReplica.Update(
            @event.NewStatus, 
            orderReplica.PaymentMethod, 
            orderReplica.IsPaid, 
            orderReplica.PaidAt, 
            @event.UpdateAt);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
