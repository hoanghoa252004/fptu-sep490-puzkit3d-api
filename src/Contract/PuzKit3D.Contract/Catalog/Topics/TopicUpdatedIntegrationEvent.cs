using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Topics;

public sealed record TopicUpdatedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid TopicId,
    string Name,
    string Slug,
    Guid? ParentId,
    string? Description,
    DateTime UpdatedAt) : IntegrationEvent(EventId, OccurredOn);
