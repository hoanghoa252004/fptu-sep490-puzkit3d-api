using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts.DomainEvents;

public sealed record PartnerProductCreatedDomainEvent
(
    Guid ProductId,
    Guid PartnerId,
    string Name,
    decimal ReferencePrice,
    int Quantity,
    string ThumbnailUrl,
    string PreviewAsset,
    string Slug,
    string? Description,
    bool IsActive,
    DateTime CreatedAt
) : DomainEvent;
