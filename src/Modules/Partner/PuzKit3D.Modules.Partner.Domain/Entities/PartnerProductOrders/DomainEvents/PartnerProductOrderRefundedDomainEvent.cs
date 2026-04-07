using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders.DomainEvents;

public sealed record PartnerProductOrderRefundedDomainEvent (
    Guid OrderId,
    string Code,
    Guid CustomerId,
    decimal GrandTotalAmount,
    int UserCoinAmount,
    string PaymentMethod,
    decimal PercentRefund,
    DateTime UpdatedAt) : DomainEvent;