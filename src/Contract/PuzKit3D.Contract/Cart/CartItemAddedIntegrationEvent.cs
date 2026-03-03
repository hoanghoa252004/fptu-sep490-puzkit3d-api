using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.Cart;

public sealed record CartItemAddedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid CartId,
    Guid UserId,
    Guid ItemId,
    int Quantity,
    decimal? UnitPrice) : IIntegrationEvent;
