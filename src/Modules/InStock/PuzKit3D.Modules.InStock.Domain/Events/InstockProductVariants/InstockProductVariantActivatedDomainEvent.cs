using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Events.InstockProductVariants;

public sealed record InstockProductVariantActivatedDomainEvent(
    Guid VariantId,
    bool IsActive) : DomainEvent;
