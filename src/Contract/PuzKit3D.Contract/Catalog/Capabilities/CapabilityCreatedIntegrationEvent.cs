using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Capabilities;

public sealed record CapabilityCreatedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid CapabilityId,
    string Name,
    string Slug,
    decimal FactorPercentage,
    string? Description,
    bool IsActive,
    DateTime CreatedAt) : IntegrationEvent(EventId, OccurredOn);
