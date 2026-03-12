using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockProductVariants;

public sealed record InstockProductVariantDeletedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid VariantId,
    Guid ProductId) : IIntegrationEvent;
