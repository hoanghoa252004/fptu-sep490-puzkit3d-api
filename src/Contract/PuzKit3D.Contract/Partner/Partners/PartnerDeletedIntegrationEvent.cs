using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Partner.Partners;

public sealed record PartnerDeletedIntegrationEvent
(
    Guid Id,
    DateTime OccurredOn,
    Guid PartnerId,
    DateTime UpdatedAt
) : IIntegrationEvent;