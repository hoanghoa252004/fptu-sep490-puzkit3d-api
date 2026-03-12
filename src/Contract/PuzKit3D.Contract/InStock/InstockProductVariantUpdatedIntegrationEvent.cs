using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock;

public sealed record InstockProductVariantUpdatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid VariantId,
    Guid ProductId,
    string Sku,
    string Color,
    int AssembledLengthMm,
    int AssembledWidthMm,
    int AssembledHeightMm,
    bool IsActive) : IIntegrationEvent;
