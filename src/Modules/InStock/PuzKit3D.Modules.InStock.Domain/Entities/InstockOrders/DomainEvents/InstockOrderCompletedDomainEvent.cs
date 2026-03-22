using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;

public sealed record InstockOrderCompletedDomainEvent(
    Guid OrderId,
    string Code,
    Guid CustomerId,
    List<OrderDetailInfo> OrderDetails,
    DateTime CompletedAt) : DomainEvent;

public sealed record OrderDetailInfo(
    Guid OrderDetailId,
    Guid VariantId);
