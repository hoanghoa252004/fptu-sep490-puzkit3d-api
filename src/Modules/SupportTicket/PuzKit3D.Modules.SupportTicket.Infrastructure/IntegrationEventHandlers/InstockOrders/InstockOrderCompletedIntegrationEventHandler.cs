using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Application.UnitOfWork;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.CompletedOrderItemReplicas;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.CompletedOrderReplicas;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.SupportTicket.Infrastructure.IntegrationEventHandlers.InstockOrders;

internal sealed class InstockOrderCompletedIntegrationEventHandler
    : IIntegrationEventHandler<InstockOrderCompletedIntegrationEvent>
{
    private readonly ICompletedOrderReplicaRepository _replicaRepository;
    private readonly ICompletedOrderItemReplicaRepository _itemReplicaRepository;
    private readonly ISupportTicketUnitOfWork _unitOfWork;

    public InstockOrderCompletedIntegrationEventHandler(
        ICompletedOrderReplicaRepository replicaRepository,
        ICompletedOrderItemReplicaRepository itemReplicaRepository,
        ISupportTicketUnitOfWork unitOfWork)
    {
        _replicaRepository = replicaRepository;
        _itemReplicaRepository = itemReplicaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        InstockOrderCompletedIntegrationEvent @event,
        CancellationToken cancellationToken = default)
    {
        // Create a CompletedOrderReplica with order code, type, and customer ID
        var replica = CompletedOrderReplica.Create(
            @event.Id,
            @event.Code,
            "Instock",
            @event.CustomerId);

        await _replicaRepository.AddAsync(replica, cancellationToken);

        // Create CompletedOrderItemReplica for each order detail
        var itemReplicas = new List<CompletedOrderItemReplica>();
        foreach (var orderDetail in @event.OrderDetails)
        {
            var itemReplica = CompletedOrderItemReplica.Create(
                orderDetail.OrderDetailId,
                @event.Id,
                orderDetail.ProductId,
                orderDetail.VariantId,
                orderDetail.Quantity);

            itemReplicas.Add(itemReplica);
        }

        // Save all item replicas
        if (itemReplicas.Count > 0)
        {
            await _itemReplicaRepository.AddRangeAsync(itemReplicas, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

