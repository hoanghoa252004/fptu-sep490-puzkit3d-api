using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.Feedback.Application.UnitOfWork;
using PuzKit3D.Modules.Feedback.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Feedback.Infrastructure.IntegrationEventHandlers.InstockOrders;

internal sealed class InstockOrderStatusChangedIntegrationEventHandler
    : IIntegrationEventHandler<InstockOrderStatusChangedIntegrationEvent>
{
    private readonly FeedbackDbContext _dbContext;
    private readonly IFeedbackUnitOfWork _unitOfWork;


    public InstockOrderStatusChangedIntegrationEventHandler(
        FeedbackDbContext dbContext,
        IFeedbackUnitOfWork unitOfWork)
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

