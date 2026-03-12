using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockPrices;

public sealed record InstockPriceCreatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid PriceId,
    string Name,
    DateTime EffectiveFrom,
    DateTime EffectiveTo,
    int Priority,
    bool IsActive) : IIntegrationEvent;
