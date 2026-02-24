using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.InStock;

public sealed record OrderCreatedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid OrderId,
    Guid UserId,
    decimal TotalMoney,
    DateTime CreatedAt) : IIntegrationEvent;
