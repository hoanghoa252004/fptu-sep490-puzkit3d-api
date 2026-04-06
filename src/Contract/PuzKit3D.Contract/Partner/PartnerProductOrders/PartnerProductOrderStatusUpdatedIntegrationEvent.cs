using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Partner.PartnerProductOrders;

public sealed record PartnerProductOrderStatusUpdatedIntegrationEvent (
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId,
    string Code,
    Guid CustomerId,
    string NewStatus,
    DateTime UpdateAt) : IIntegrationEvent;
