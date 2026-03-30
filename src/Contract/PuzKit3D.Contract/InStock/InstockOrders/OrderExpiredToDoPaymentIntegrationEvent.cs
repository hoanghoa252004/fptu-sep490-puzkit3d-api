using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock.InstockOrders;

public sealed record OrderExpiredToDoPaymentIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId) : IIntegrationEvent;
