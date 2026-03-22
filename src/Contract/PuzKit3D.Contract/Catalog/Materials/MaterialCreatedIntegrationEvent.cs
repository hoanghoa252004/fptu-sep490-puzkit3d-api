using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Materials;

public sealed record MaterialCreatedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid MaterialId,
    string Name,
    string Slug,
    string? Description,
    bool IsActive,
    DateTime CreatedAt) : IntegrationEvent(EventId, OccurredOn);
