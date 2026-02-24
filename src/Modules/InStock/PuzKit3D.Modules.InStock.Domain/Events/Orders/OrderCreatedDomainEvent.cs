using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Events.Orders;

public sealed record OrderCreatedDomainEvent(
    Guid OrderId,
    Guid UserId,
    decimal TotalMoney,
    DateTime CreatedAt) : DomainEvent;
