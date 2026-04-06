using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.Feedback.Application.UnitOfWork;
using PuzKit3D.Modules.Feedback.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Feedback.Infrastructure.IntegrationEventHandlers.PartnerProductOrders;

internal sealed class PartnerProductOrderStatusUpdatedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductOrderStatusUpdatedIntegrationEvent>
{
    private readonly FeedbackDbContext _dbContext;
    private readonly IFeedbackUnitOfWork _unitOfWork;

    public PartnerProductOrderStatusUpdatedIntegrationEventHandler(
        FeedbackDbContext dbContext, 
        IFeedbackUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }
    public async Task HandleAsync(PartnerProductOrderStatusUpdatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var orderReplica = await _dbContext.OrderReplicas.FindAsync(
            new object[] { @event.OrderId }, cancellationToken);

        if (orderReplica is null) { return; }

        orderReplica.Update(@event.NewStatus);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
