using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Partner.PartnerProductOrders;

public sealed record PartnerProductOrderRefundedIntegrationEvent (
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId,
    string OrderCode,
    Guid CustomerId,
    decimal GrandTotalAmount,
    decimal UserCoinAmount,
    string PaymentMethod,
    decimal PercentRefund,
    DateTime UpdatedAt) : IIntegrationEvent;