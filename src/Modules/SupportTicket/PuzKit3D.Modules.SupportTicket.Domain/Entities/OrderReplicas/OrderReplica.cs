using PuzKit3D.SharedKernel.Domain;
using System.Net.NetworkInformation;

namespace PuzKit3D.Modules.SupportTicket.Domain.Entities.OrderReplicas;

public sealed class OrderReplica : Entity<Guid>
{
    public string Type { get; private set; } = null!; 
    public string Code { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string Status { get; private set; }

    private OrderReplica(
        Guid id,
        string type,
        Guid customerId,
        string code,
        DateTime createdAt,
        DateTime updatedAt,
        string status) : base(id)
    {
        Type = type;
        CustomerId = customerId;
        Code = code;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Status = status;
    }

    private OrderReplica() : base()
    {
    }

    public static OrderReplica Create(
        Guid id,
        string type,
        Guid customerId,
        string code,
        string status)
    {
        return new OrderReplica(
            id,
            type,
            customerId,
            code,
            DateTime.UtcNow,
            DateTime.UtcNow,
            status);
    }

    public void Update(
        string status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}
