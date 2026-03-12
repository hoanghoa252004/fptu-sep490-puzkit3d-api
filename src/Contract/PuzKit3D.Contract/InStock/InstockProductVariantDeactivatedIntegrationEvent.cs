using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock;

public sealed record InstockProductVariantDeactivatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid VariantId,
    Guid ProductId) : IIntegrationEvent;
