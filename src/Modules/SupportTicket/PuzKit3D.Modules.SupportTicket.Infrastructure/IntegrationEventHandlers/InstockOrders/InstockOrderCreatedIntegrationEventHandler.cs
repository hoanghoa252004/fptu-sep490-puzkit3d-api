using Microsoft.EntityFrameworkCore;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.SupportTicket.Application.UnitOfWork;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.OrderReplicas;
using PuzKit3D.Modules.SupportTicket.Persistence;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.SupportTicket.Infrastructure.IntegrationEventHandlers.InstockOrders;

internal sealed class InstockOrderCreatedIntegrationEventHandler
    : IIntegrationEventHandler<InstockOrderCreatedIntegrationEvent>
{
    private readonly SupportTicketDbContext _dbContext;
    private readonly ISupportTicketUnitOfWork _unitOfWork;

    public InstockOrderCreatedIntegrationEventHandler(
        SupportTicketDbContext dbContext,
        ISupportTicketUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        InstockOrderCreatedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        var replica = OrderReplica.Create(
            @event.OrderId,
            "Instock",
            @event.CustomerId,
            @event.Code,
            @event.Status,
            @event.GrandTotalAmount);

        await _dbContext.OrderReplicas.AddAsync(replica, cancellationToken);

        foreach (var orderDetail in @event.OrderDetails)
        {

            var orderDetailReplica = OrderDetailReplica.Create(
                orderDetail.OrderDetailId,
                @event.OrderId,
                orderDetail.ProductId,
                orderDetail.VariantId,
                orderDetail.Quantity);

            await _dbContext.OrderDetailReplicas.AddAsync(orderDetailReplica, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
