using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Materials;

public sealed record MaterialCreatedIntegrationEvent(
    Guid EventId,
    DateTime OccurredOn,
    Guid MaterialId,
    string Name,
    string Slug,
    decimal FactorPercentage,
    decimal BasePrice,
    string? Description,
    bool IsActive,
    DateTime CreatedAt) : IntegrationEvent(EventId, OccurredOn);
