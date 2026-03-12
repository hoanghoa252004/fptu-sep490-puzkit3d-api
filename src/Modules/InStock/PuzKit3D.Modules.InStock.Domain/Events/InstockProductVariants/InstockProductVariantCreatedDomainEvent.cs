using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Events.InstockProductVariants;

public sealed record InstockProductVariantCreatedDomainEvent(
    Guid VariantId,
    Guid ProductId,
    string Sku,
    string Color,
    int AssembledLengthMm,
    int AssembledWidthMm,
    int AssembledHeightMm,
    bool IsActive) : DomainEvent;
