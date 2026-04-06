using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders.DomainEvents;

public sealed record CoinUsedDomainEvent(
    Guid OrderId,
    string OrderCode,
    Guid CustomerId,
    decimal UsedCoinAmount,
    DateTime CreatedAt) : DomainEvent;
