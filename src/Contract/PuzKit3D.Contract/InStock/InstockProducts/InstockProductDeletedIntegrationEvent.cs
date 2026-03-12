using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockProducts;

public sealed record InstockProductDeletedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid ProductId) : IIntegrationEvent;
