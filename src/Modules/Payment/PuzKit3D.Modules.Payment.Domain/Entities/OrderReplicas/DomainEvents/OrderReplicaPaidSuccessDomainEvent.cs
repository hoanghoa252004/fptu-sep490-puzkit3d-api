using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Payment.Domain.Entities.OrderReplicas.DomainEvents;

public sealed record OrderReplicaPaidSuccessDomainEvent(
    Guid OrderReplicaId,
    string OrderType,
    string Code,
    Guid CustomerId,
    decimal Amount,
    DateTime PaidAt) : DomainEvent;
