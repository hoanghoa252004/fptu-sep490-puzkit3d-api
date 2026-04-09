using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Topics;

public sealed record TopicUpdatedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid TopicId,
    string Name,
    string Slug,
    Guid? ParentId,
    decimal FactorPercentage,
    string? Description,
    DateTime UpdatedAt,
    bool IsActive) : IntegrationEvent(EventId, OccurredOn);
