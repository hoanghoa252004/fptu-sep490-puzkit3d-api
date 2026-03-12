using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Payment.Domain.Entities.OrderReplicas;

public sealed class OrderReplica : Entity<Guid>
{
    public string Type { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public Guid CustomerId { get; private set; }
    public decimal Amount { get; private set; }
    public string Status { get; private set; } = null!;
    public string PaymentMethod { get; private set; } = null!;
    public bool IsPaid { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private OrderReplica(
        Guid id,
        string type,
        string code,
        Guid customerId,
        decimal amount,
        string status,
        string paymentMethod,
        bool isPaid,
        DateTime? paidAt,
        DateTime createdAt,
        DateTime updatedAt) : base(id)
    {
        Type = type;
        Code = code;
        CustomerId = customerId;
        Amount = amount;
        Status = status;
        PaymentMethod = paymentMethod;
        IsPaid = isPaid;
        PaidAt = paidAt;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    private OrderReplica() : base()
    {
    }

    public static OrderReplica Create(
        Guid id,
        string type,
        string code,
        Guid customerId,
        decimal amount,
        string status,
        string paymentMethod,
        bool isPaid,
        DateTime? paidAt,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new OrderReplica(
            id,
            type,
            code,
            customerId,
            amount,
            status,
            paymentMethod,
            isPaid,
            paidAt,
            createdAt,
            updatedAt);
    }

    public void Update(
        string status,
        string paymentMethod,
        bool isPaid,
        DateTime? paidAt,
        DateTime updatedAt)
    {
        Status = status;
        PaymentMethod = paymentMethod;
        IsPaid = isPaid;
        PaidAt = paidAt;
        UpdatedAt = updatedAt;
    }
}
