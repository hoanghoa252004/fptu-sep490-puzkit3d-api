using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Events.Carts;

public sealed record CartClearedDomainEvent(
    Guid CartId,
    Guid UserId) : DomainEvent;
