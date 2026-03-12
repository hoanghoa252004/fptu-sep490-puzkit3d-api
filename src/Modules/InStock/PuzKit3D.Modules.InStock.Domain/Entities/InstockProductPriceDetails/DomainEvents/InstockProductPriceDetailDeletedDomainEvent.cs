using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails.DomainEvents;

public sealed record InstockProductPriceDetailDeletedDomainEvent(
    Guid PriceDetailId,
    Guid PriceId,
    Guid VariantId) : DomainEvent;
