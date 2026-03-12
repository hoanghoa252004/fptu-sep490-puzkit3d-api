using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Events.InstockPrices;

public sealed record InstockPriceUpdatedDomainEvent(
    Guid PriceId,
    string Name,
    DateTime EffectiveFrom,
    DateTime EffectiveTo,
    int Priority,
    bool IsActive) : DomainEvent;
