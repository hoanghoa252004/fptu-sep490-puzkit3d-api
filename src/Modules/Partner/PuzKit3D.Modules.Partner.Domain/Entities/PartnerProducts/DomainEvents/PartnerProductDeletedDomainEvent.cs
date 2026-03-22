using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts.DomainEvents;

public sealed record PartnerProductDeletedDomainEvent
(
    Guid ProductId, 
    DateTime UpdatedAt
) : DomainEvent;
