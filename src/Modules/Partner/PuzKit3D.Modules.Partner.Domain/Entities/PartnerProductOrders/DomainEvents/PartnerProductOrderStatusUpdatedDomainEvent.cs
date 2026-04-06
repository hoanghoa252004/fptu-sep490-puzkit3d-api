
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders.DomainEvents;

public sealed record PartnerProductOrderStatusUpdatedDomainEvent (
    Guid OrderId,
    string Code,
    Guid CustomerId,
    PartnerProductOrderStatus NewStatus,
    DateTime UpdatedAt) : DomainEvent;
