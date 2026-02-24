using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Events.Products;

public sealed record ProductStockChangedDomainEvent(
    Guid ProductId,
    int OldStock,
    int NewStock,
    int Quantity) : DomainEvent;
