using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;

public sealed class SupportTicketReplica : Entity<Guid>
{
    public string Code { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public Guid OrderId { get; private set; }
    public string Type { get; private set; } = null!;
    public string Status { get; private set; } = null!;
    public string Reason { get; private set; } = null!;
    public string Proof { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<SupportTicketDetailReplica> _details = new();
    public IReadOnlyCollection<SupportTicketDetailReplica> Details => _details.AsReadOnly();

    private SupportTicketReplica(
        Guid id,
        Guid userId,
        Guid orderId,
        string type,
        string status,
        string reason,
        string proof,
        DateTime createdAt,
        DateTime updatedAt,
        string code) : base(id)
    {
        UserId = userId;
        OrderId = orderId;
        Type = type;
        Status = status;
        Reason = reason;
        Proof = proof;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Code = code;
    }

    private SupportTicketReplica() : base()
    {
    }

    public static SupportTicketReplica Create(
        Guid id,
        string code,
        Guid userId,
        Guid orderId,
        string type,
        string status,
        string reason,
        string proof,
        DateTime createdAt,
        DateTime updatedAt)
    {
        return new SupportTicketReplica(
            id,
            userId,
            orderId,
            type,
            status,
            reason,
            proof,
            createdAt,
            updatedAt,
            code);
    }

    public void UpdateStatus(string newStatus, DateTime updatedAt)
    {
        Status = newStatus;
        UpdatedAt = updatedAt;
    }

    public void AddDetail(SupportTicketDetailReplica detail)
    {
        _details.Add(detail);
    }

    public void ClearDetails()
    {
        _details.Clear();
    }
}
