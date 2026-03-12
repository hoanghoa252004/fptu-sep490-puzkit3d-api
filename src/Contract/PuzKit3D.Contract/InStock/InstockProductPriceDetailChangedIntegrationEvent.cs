using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock;

public sealed record InstockProductPriceDetailChangedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid PriceDetailId,
    Guid PriceId,
    Guid VariantId,
    decimal UnitPrice,
    bool IsActive) : IIntegrationEvent;
