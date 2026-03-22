using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Partner.PartnerProducts;

public sealed record PartnerProductCreatedIntegrationEvent
(
    Guid Id,
    DateTime OccurredOn,
    Guid ProductId,
    Guid PartnerId,
    string Name,
    decimal ReferencePrice,
    string ThumbnailUrl,
    string PreviewAsset,
    string Slug,
    string? Description,
    bool IsActive,
    DateTime CreatedAt) : IIntegrationEvent;
