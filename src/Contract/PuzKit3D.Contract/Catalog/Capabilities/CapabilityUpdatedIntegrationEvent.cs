using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Capabilities;

public sealed record CapabilityUpdatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid CapabilityId,
    string Name,
    string Slug,
    decimal FactorPercentage,
    string? Description,
    DateTime UpdatedAt,
    bool IsActive) : IIntegrationEvent;
