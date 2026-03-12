using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockProductPriceDetails;

public sealed record InstockProductPriceDetailDeletedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid PriceDetailId,
    Guid PriceId,
    Guid VariantId) : IIntegrationEvent;
