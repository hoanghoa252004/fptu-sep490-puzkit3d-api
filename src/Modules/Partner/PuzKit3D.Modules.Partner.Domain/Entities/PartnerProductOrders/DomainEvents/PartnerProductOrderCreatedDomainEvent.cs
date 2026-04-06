using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders.DomainEvents;

public sealed record PartnerProductOrderCreatedDomainEvent(
    Guid OrderId,
    Guid CustomerId,
    string Code,
    decimal GrandTotalAmount,
    string Status,
    string PaymentMethod,
    int UsedCoinAmount,
    bool IsPaid,
    DateTime? PaidAt,
    DateTime CreatedAt,
    List<PartnerProductOrderItemInfo> Items
) : DomainEvent;

public sealed record PartnerProductOrderItemInfo(
    Guid OrderDetailId,
    Guid PartnerProductId,
    int Quantity,
    string? ProductName
);