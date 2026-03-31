using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;

public sealed record InstockOrderStatusChangedDomainEvent(
    Guid OrderId,
    string Code,
    Guid CustomerId,
    InstockOrderStatus NewStatus,
    DateTime ChangedAt,
    string PaymentMethod,
    decimal GrandTotalAmount,
    int UsedCoinAmount) : DomainEvent;
