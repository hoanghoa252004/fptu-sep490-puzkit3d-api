using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;

public sealed record InstockOrderCancelledDomainEvent(
    Guid OrderId,
    string OrderCode,
    Guid CustomerId,
    List<OrderCancelledDetailInfo> OrderDetails,
    DateTime CancelledAt) : DomainEvent;

public sealed record OrderCancelledDetailInfo(
    Guid OrderDetailId,
    Guid VariantId,
    int Quantity);
