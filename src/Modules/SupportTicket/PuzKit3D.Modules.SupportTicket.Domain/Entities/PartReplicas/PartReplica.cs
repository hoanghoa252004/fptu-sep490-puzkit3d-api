using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.SupportTicket.Domain.Entities.PartReplicas;

public sealed class PartReplica : Entity<Guid>
{
    public Guid PartId { get; private set; }
    public string Name { get; private set; } = null!;
    public string PartType { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public int Quantity { get; private set; }
    public Guid InstockProductId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private PartReplica(
        Guid id,
        Guid partId,
        string name,
        string partType,
        string code,
        int quantity,
        Guid instockProductId) : base(id)
    {
        PartId = partId;
        Name = name;
        PartType = partType;
        Code = code;
        Quantity = quantity;
        InstockProductId = instockProductId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    private PartReplica() : base()
    {
    }

    public static PartReplica Create(
        Guid partId,
        string name,
        string partType,
        string code,
        int quantity,
        Guid instockProductId)
    {
        return new PartReplica(Guid.NewGuid(), partId, name, partType, code, quantity, instockProductId);
    }

    public void Update(
        string name,
        string partType,
        string code,
        int quantity)
    {
        Name = name;
        PartType = partType;
        Code = code;
        Quantity = quantity;
        UpdatedAt = DateTime.UtcNow;
    }
}
