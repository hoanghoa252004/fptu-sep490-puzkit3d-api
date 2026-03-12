using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Events.InstockProductPriceDetails;

public sealed record InstockProductPriceDetailCreatedDomainEvent(
    Guid PriceDetailId,
    Guid PriceId,
    Guid VariantId,
    decimal UnitPrice,
    bool IsActive) : DomainEvent;
