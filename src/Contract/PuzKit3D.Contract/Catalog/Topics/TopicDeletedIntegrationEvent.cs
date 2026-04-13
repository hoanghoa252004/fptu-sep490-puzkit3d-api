using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Topics;

public sealed record TopicDeletedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid TopicId,
    DateTime DeletedAt) : IIntegrationEvent;
