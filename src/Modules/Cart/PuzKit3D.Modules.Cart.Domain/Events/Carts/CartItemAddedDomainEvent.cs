using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Events.Carts;

public sealed record CartItemAddedDomainEvent(
    Guid CartId,
    Guid CartItemId,
    Guid ItemId,
    int Quantity) : DomainEvent;
