using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.Feedback.Application.UnitOfWork;
using PuzKit3D.Modules.Feedback.Domain.Entities.OrderReplicas;
using PuzKit3D.Modules.Feedback.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Feedback.Infrastructure.IntegrationEventHandlers.PartnerProductOrders;

internal sealed class PartnerProductOrderCreatedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductOrderCreatedIntegrationEvent>
{
    private readonly FeedbackDbContext _dbContext;
    private readonly IFeedbackUnitOfWork _unitOfWork;

    public PartnerProductOrderCreatedIntegrationEventHandler(
        FeedbackDbContext dbContext, 
        IFeedbackUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        PartnerProductOrderCreatedIntegrationEvent @event, 
        CancellationToken cancellationToken = default)
    {
        var replica = OrderReplica.Create(
            @event.OrderId,
            "Partner",
            @event.CustomerId,
            @event.Code,
            @event.Status);

        await _dbContext.OrderReplicas.AddAsync(replica, cancellationToken);

        foreach (var item in @event.Details)
        {
            var orderItemReplica = OrderDetailReplica.Create(
                item.OrderDetailId,
                @event.OrderId,
                item.PartnerProductId,
                null,
                item.Quantity);

            await _dbContext.OrderDetailReplicas.AddAsync(orderItemReplica, cancellationToken);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
