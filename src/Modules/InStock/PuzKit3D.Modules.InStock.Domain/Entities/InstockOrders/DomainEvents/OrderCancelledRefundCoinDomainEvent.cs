using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;

public sealed record OrderCancelledRefundCoinDomainEvent(
    Guid OrderId,
    string OrderCode,
    Guid CustomerId,
    decimal GrandTotalAmount,
    int UsedCoinAmount,
    string PaymentMethod,
    DateTime CancelledAt) : DomainEvent;
