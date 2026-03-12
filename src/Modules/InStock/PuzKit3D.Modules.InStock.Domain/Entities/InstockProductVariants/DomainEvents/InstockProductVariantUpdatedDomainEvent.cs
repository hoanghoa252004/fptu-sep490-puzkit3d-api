using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants.DomainEvents;

public sealed record InstockProductVariantUpdatedDomainEvent(
    Guid VariantId,
    Guid ProductId,
    string Sku,
    string Color,
    int AssembledLengthMm,
    int AssembledWidthMm,
    int AssembledHeightMm,
    bool IsActive) : DomainEvent;
