using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.InStock.Domain.Events.Products;

public sealed record ProductCreatedDomainEvent(
    Guid ProductId,
    string Name,
    decimal Price,
    int InitialStock) : DomainEvent;
