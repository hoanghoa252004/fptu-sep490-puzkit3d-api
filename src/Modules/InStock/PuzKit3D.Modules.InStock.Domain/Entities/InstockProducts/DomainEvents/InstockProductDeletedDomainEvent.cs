using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts.DomainEvents;

public sealed record InstockProductDeletedDomainEvent(
    Guid ProductId) : DomainEvent;
