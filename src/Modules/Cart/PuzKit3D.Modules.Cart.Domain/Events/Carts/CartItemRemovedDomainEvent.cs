using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Events.Carts;

public sealed record CartItemRemovedDomainEvent(
    Guid CartId,
    Guid CartItemId,
    Guid ItemId) : DomainEvent;
