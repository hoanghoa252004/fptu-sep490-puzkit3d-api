using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Events.InstockProducts;

public sealed record InstockProductCreatedDomainEvent(
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
    Guid AssemblyMethodId,
    Guid CapabilityId,
    Guid MaterialId,
    bool IsActive,
    DateTime CreatedAt) : DomainEvent;
