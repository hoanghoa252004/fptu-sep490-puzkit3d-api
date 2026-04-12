using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts.DomainEvents;

public sealed record InstockProductUpdatedDomainEvent(
    Guid ProductId,
    string Code,
    string Slug,
    string Name,
    int TotalPieceCount,
    string DifficultLevel,
    int EstimatedBuildTime,
    string ThumbnailUrl,
    string PreviewAsset,
    string? Description,
    Guid TopicId,
    Guid MaterialId,
    bool IsActive,
    DateTime UpdatedAt) : DomainEvent;
