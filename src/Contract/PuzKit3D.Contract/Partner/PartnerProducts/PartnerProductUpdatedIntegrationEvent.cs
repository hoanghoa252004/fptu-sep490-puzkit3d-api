using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Partner.PartnerProducts;

public sealed record PartnerProductUpdatedIntegrationEvent
(
    Guid Id,
    DateTime OccurredOn,
    Guid ProductId,
    Guid PartnerId,
    string Name,
    decimal ReferencePrice,
    int Quantity,
    string ThumbnailUrl,
    string PreviewAsset,
    string Slug,
    string? Description,
    DateTime UpdatedAt) : IIntegrationEvent;
