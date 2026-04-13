using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockProducts;

public sealed record InstockProductUpdatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
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
    DateTime UpdatedAt) : IIntegrationEvent;
