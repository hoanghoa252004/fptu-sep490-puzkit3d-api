using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices.DomainEvents;

public sealed record InstockPriceCreatedDomainEvent(
    Guid PriceId,
    string Name,
    DateTime EffectiveFrom,
    DateTime EffectiveTo,
    int Priority,
    bool IsActive) : DomainEvent;
