using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Delivery;

public sealed record OrderReturnedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId) : IIntegrationEvent;
