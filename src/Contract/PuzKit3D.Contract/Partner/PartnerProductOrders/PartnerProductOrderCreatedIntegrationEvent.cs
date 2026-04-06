using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Partner.PartnerProductOrders;

public sealed record PartnerProductOrderCreatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
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
    List<PartnerProductOrderItem> Details
) : IIntegrationEvent;

public sealed record PartnerProductOrderItem(
    Guid OrderDetailId,
    Guid PartnerProductId,
    int Quantity,
    string? ProductName
);