using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts.DomainEvents;

public sealed record PartnerProductActivatedDomainEvent 
    (Guid ProductId,
    DateTime UpdatedAt) : DomainEvent;
