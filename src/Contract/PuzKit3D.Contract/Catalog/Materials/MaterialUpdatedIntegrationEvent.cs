using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Catalog.Materials;

public sealed record MaterialUpdatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid MaterialId,
    string Name,
    string Slug,
    decimal FactorPercentage,
    decimal BasePrice,
    string? Description,
    DateTime UpdatedAt,
    bool IsActive) : IIntegrationEvent;
