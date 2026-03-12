using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Events.InstockProductPriceDetails;

public sealed record InstockProductPriceDetailActivatedDomainEvent(
    Guid PriceDetailId,
    bool IsActive) : DomainEvent;
