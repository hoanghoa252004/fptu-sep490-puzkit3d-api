using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Partner.PartnerProductOrders;

public sealed record PartnerProductCoinUsedIntegrationEvent (
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId,
    string OrderCode,
    Guid CustomerId,
    decimal UsedCoinAmount,
    DateTime CreatedAt) : IIntegrationEvent;
