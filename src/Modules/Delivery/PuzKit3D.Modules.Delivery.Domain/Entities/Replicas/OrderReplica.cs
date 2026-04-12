using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;

public sealed class OrderReplica : Entity<Guid>
{
    public string Type { get; private set; } = null!; 
    public string Code { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public decimal GrandTotalAmount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string Status { get; private set; } = null!;

    private OrderReplica(
        Guid id,
        string type,
        Guid customerId,
        string code,
        DateTime createdAt,
        DateTime updatedAt,
        string status,
        decimal grandTotalAmount) : base(id)
    {
        Type = type;
        CustomerId = customerId;
        Code = code;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Status = status;
        GrandTotalAmount = grandTotalAmount;
    }

    private OrderReplica() : base()
    {
    }

    public static OrderReplica Create(
        Guid id,
        string type,
        Guid customerId,
        string code,
        string status,
        decimal grandTotalAmount)
    {
        return new OrderReplica(
            id,
            type,
            customerId,
            code,
            DateTime.UtcNow,
            DateTime.UtcNow,
            status,
            grandTotalAmount);
    }


    public void Update(
        string status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}
