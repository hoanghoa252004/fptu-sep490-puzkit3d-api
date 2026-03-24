using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;

public sealed record OrderCancelledRefundCoinDomainEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId,
    string OrderCode,
    Guid CustomerId,
    decimal GrandTotalAmount,
    DateTime CancelledAt) : IDomainEvent;
