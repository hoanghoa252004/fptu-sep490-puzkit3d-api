using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.SupportTicket.Application.UnitOfWork;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.OrderReplicas;
using PuzKit3D.Modules.SupportTicket.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.SupportTicket.Infrastructure.IntegrationEventHandlers.PartnerProductOrders;

internal sealed class PartnerProductOrderCreatedIntegrationEventHandler
    : IIntegrationEventHandler<PartnerProductOrderCreatedIntegrationEvent>
{
    private readonly SupportTicketDbContext _dbContext;
    private readonly ISupportTicketUnitOfWork _unitOfWork;

    public PartnerProductOrderCreatedIntegrationEventHandler(
        SupportTicketDbContext dbContext, 
        ISupportTicketUnitOfWork unitOfWork)
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
            @event.Status,
            @event.GrandTotalAmount);

        await _dbContext.OrderReplicas.AddAsync(replica, cancellationToken);

        foreach (var item in @event.Details)
        {
            var itemReplica = OrderDetailReplica.Create(
                item.OrderDetailId,
                @event.OrderId,
                item.PartnerProductId,
                null,
                item.Quantity);
            await _dbContext.OrderDetailReplicas.AddAsync(itemReplica, cancellationToken);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
