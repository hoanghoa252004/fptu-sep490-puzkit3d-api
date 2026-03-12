using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Events.InstockProducts;

public sealed record InstockProductDeletedDomainEvent(
    Guid ProductId) : DomainEvent;
