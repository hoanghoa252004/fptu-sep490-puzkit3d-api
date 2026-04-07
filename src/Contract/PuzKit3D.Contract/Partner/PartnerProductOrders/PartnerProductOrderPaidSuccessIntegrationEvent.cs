using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Partner.PartnerProductOrders;

public sealed record PartnerProductOrderPaidSuccessIntegrationEvent (
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId,
    string Code,
    Guid CustomerId,
    decimal GrandTotalAmount,
    DateTime PaidAt) : IIntegrationEvent;
