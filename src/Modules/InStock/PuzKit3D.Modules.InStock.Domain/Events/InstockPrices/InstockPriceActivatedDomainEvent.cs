using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Events.InstockPrices;

public sealed record InstockPriceActivatedDomainEvent(
    Guid PriceId,
    bool IsActive) : DomainEvent;
