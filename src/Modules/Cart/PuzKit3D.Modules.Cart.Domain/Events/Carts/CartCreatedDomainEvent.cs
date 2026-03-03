using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Cart.Domain.Events.Carts;

public sealed record CartCreatedDomainEvent(
    Guid CartId,
    Guid UserId,
    Guid CartTypeId) : DomainEvent;
