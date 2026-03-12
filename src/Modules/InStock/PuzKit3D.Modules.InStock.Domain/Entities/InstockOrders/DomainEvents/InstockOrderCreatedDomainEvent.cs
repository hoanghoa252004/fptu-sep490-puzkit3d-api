using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;

public sealed record InstockOrderCreatedDomainEvent(
    Guid OrderId,
    string Code,
    Guid CustomerId,
    List<Guid> CartItemIds,
    decimal GrandTotalAmount,
    DateTime CreatedAt,
    string Status,
    string PaymentMethod,
    bool IsPaid,
    DateTime? PaidAt) : DomainEvent;

