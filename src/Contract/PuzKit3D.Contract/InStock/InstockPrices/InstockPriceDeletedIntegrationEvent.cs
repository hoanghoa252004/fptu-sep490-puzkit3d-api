using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockPrices;

public sealed record InstockPriceDeletedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid PriceId) : IIntegrationEvent;
