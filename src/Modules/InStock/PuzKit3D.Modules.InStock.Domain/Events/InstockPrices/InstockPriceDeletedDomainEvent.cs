using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Events.InstockPrices;

public sealed record InstockPriceDeletedDomainEvent(
    Guid PriceId) : DomainEvent;
