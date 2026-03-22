using PuzKit3D.Modules.SupportTicket.Domain.Entities.CompletedOrderItemReplicas;
using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.SupportTicket.Domain.Entities.CompletedOrderReplicas;

public sealed class CompletedOrderReplica : Entity<Guid>
{
    private readonly List<CompletedOrderItemReplica> _orderItems = new();
    public string Code { get; private set; } = null!;
    public string Type { get; private set; } = null!;
    public Guid CustomerId { get; private set; }

    public IReadOnlyCollection<CompletedOrderItemReplica> OrderItems => _orderItems.AsReadOnly();

    private CompletedOrderReplica(
        Guid id,
        string code,
        string type,
        Guid customerId) : base(id)
    {
        Code = code;
        Type = type;
        CustomerId = customerId;
    }

    private CompletedOrderReplica() : base()
    {
    }

    public static CompletedOrderReplica Create(
        Guid id,
        string code,
        string type,
        Guid customerId)
    {
        return new CompletedOrderReplica(
            id,
            code,
            type,
            customerId);
    }
}
