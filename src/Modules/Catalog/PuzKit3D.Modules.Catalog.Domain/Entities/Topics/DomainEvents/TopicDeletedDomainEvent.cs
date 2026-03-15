using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Topics.DomainEvents;

public sealed record TopicDeletedDomainEvent(
    Guid TopicId,
    DateTime DeletedAt) : DomainEvent;
