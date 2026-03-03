using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Cart;

public sealed record CartClearedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid CartId,
    Guid UserId) : IIntegrationEvent;
