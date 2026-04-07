using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequestDetails;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests.DomainEvents;

public sealed record PartnerProductRequestCreatedDomainEvent
(
    Guid PartnerProductRequestId,
    Guid CustomerId,
    Guid PartnerId,
    List<PartnerProductRequestDetailDto> Items,
    DateTime CreatedAt
) : DomainEvent;