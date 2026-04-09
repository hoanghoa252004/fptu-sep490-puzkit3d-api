using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Catalog.Domain.Entities.Topics.DomainEvents;

public sealed record TopicCreatedDomainEvent(
    Guid TopicId,
    string Name,
    string Slug,
    Guid? ParentId,
    decimal FactorPercentage,
    string? Description,
    bool IsActive,
    DateTime CreatedAt) : DomainEvent;
