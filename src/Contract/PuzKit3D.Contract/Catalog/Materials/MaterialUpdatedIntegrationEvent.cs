using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Materials;

public sealed record MaterialUpdatedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid MaterialId,
    string Name,
    string Slug,
    decimal FactorPercentage,
    decimal BasePrice,
    string? Description,
    DateTime UpdatedAt) : IntegrationEvent(EventId, OccurredOn);
