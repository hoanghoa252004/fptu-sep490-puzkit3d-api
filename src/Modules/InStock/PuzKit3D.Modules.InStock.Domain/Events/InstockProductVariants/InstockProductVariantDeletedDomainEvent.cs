using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Events.InstockProductVariants;

public sealed record InstockProductVariantDeletedDomainEvent(
    Guid VariantId,
    Guid ProductId) : DomainEvent;
