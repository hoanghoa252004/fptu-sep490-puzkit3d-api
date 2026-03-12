using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails.DomainEvents;

public sealed record InstockProductPriceDetailUpdatedDomainEvent(
    Guid PriceDetailId,
    Guid PriceId,
    Guid VariantId,
    decimal UnitPrice,
    bool IsActive) : DomainEvent;
