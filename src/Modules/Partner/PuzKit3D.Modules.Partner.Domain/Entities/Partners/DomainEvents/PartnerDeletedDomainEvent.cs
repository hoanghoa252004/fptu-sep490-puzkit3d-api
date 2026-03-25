using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.Partners.DomainEvents;

public sealed record PartnerDeletedDomainEvent
(
    Guid PartnerId,
    DateTime UpdatedAt
) : DomainEvent;
