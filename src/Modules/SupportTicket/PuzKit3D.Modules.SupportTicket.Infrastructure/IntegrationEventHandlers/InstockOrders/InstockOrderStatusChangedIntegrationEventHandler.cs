using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.SupportTicket.Application.UnitOfWork;
using PuzKit3D.Modules.SupportTicket.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.SupportTicket.Infrastructure.IntegrationEventHandlers.InstockOrders;

internal sealed class InstockOrderStatusChangedIntegrationEventHandler
    : IIntegrationEventHandler<InstockOrderStatusChangedIntegrationEvent>
{
    private readonly SupportTicketDbContext _dbContext;
    private readonly ISupportTicketUnitOfWork _unitOfWork;


    public InstockOrderStatusChangedIntegrationEventHandler(
        SupportTicketDbContext dbContext,
        ISupportTicketUnitOfWork unitOfWork)
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

        orderReplica.Update(
            @event.NewStatus);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

