using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;

public sealed record CoinUsedDomainEvent(
    Guid OrderId,
    string OrderCode,
    Guid CustomerId,
    decimal UsedCoinAmount,
    DateTime CreatedAt) : DomainEvent;
