using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.Feedback.Application.UnitOfWork;
using PuzKit3D.Modules.Feedback.Domain.Entities.OrderReplicas;
using PuzKit3D.Modules.Feedback.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Feedback.Infrastructure.IntegrationEventHandlers.InstockOrders;

internal sealed class InstockOrderCompletedIntegrationEventHandler
    : IIntegrationEventHandler<InstockOrderCompletedIntegrationEvent>
{
    private readonly FeedbackDbContext _dbContext;
    private readonly IFeedbackUnitOfWork _unitOfWork;

    public InstockOrderCompletedIntegrationEventHandler(
        FeedbackDbContext dbContext,
        IFeedbackUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        InstockOrderCompletedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        // Create a CompletedOrderReplica for each OrderDetail with correct ProductId and VariantId
        foreach (var orderDetail in @event.OrderDetails)
        {
            var replica = CompletedOrderReplica.Create(
                orderDetail.OrderDetailId,
                "Instock",
                @event.Code,
                @event.CustomerId,
                orderDetail.ProductId,
                orderDetail.VariantId);

            await _dbContext.CompletedOrderReplicas.AddAsync(replica, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
