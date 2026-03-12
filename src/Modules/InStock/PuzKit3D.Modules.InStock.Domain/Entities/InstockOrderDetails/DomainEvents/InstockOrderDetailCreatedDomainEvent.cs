using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrderDetails.DomainEvents;

public sealed record InstockOrderDetailCreatedDomainEvent(
    Guid OrderDetailId,
    Guid OrderId,
    Guid VariantId,
    string Sku,
    int Quantity,
    decimal UnitPrice,
    decimal TotalAmount) : DomainEvent;
