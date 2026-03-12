using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices.DomainEvents;

public sealed record InstockPriceDeletedDomainEvent(
    Guid PriceId) : DomainEvent;
