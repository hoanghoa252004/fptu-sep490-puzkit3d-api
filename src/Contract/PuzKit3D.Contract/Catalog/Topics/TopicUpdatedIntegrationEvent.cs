using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Topics;

public sealed record TopicUpdatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid TopicId,
    string Name,
    string Slug,
    Guid? ParentId,
    decimal FactorPercentage,
    string? Description,
    DateTime UpdatedAt,
    bool IsActive) : IIntegrationEvent;
