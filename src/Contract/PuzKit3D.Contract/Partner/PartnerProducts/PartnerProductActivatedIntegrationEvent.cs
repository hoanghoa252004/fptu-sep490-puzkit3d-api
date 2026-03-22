using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Partner.PartnerProducts;

public sealed record PartnerProductActivatedIntegrationEvent
(
    Guid Id,
    DateTime OccurredOn,
    Guid ProductId,
    DateTime UpdatedAt) : IIntegrationEvent;
