using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Topics;

public sealed record TopicCreatedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid TopicId,
    string Name,
    string Slug,
    Guid? ParentId,
    decimal FactorPercentage,
    string? Description,
    bool IsActive,
    DateTime CreatedAt) : IntegrationEvent(EventId, OccurredOn);
