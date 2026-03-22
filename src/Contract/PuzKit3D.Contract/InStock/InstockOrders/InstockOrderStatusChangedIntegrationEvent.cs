using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockOrders;

public sealed record InstockOrderStatusChangedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId,
    string Code,
    Guid CustomerId,
    string NewStatus,
    DateTime ChangedAt) : IIntegrationEvent;
