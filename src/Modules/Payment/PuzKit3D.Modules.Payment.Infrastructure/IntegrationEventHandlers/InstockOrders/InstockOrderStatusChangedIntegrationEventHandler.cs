using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.Payment.Application.UnitOfWork;
using PuzKit3D.Modules.Payment.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Payment.Infrastructure.IntegrationEventHandlers.InstockOrders;

internal sealed class InstockOrderStatusChangedIntegrationEventHandler
    : IIntegrationEventHandler<InstockOrderStatusChangedIntegrationEvent>
{
    private readonly PaymentDbContext _dbContext;
    private readonly IPaymentUnitOfWork _unitOfWork;

    public InstockOrderStatusChangedIntegrationEventHandler(
        PaymentDbContext dbContext,
        IPaymentUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        InstockOrderStatusChangedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var orderReplica = await _dbContext.OrderReplicas.FindAsync(
            new object[] { @event.OrderId }, cancellationToken);

        if (orderReplica is null)
        {
            return;
        }

        // Update the replica with new status directly
        orderReplica.Update(
            @event.NewStatus,
            orderReplica.PaymentMethod,
            orderReplica.IsPaid,
            orderReplica.PaidAt,
            @event.ChangedAt);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

