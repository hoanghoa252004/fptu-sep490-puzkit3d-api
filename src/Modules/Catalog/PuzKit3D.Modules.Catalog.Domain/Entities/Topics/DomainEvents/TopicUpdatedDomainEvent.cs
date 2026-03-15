using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Topics.DomainEvents;

public sealed record TopicUpdatedDomainEvent(
    Guid TopicId,
    string Name,
    string Slug,
    Guid? ParentId,
    string? Description,
    DateTime UpdatedAt) : DomainEvent;
