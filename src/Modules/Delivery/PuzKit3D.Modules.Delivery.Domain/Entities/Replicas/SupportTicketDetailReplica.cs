using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;

public sealed class SupportTicketDetailReplica : Entity<Guid>
{
    public Guid SupportTicketId { get; private set; }
    public Guid OrderItemId { get; private set; }
    public Guid? PartId { get; private set; }
    public int Quantity { get; private set; }
    public string? Note { get; private set; }

    private SupportTicketDetailReplica(
        Guid id,
        Guid supportTicketId,
        Guid orderItemId,
        Guid? partId,
        int quantity,
        string? note) : base(id)
    {
        SupportTicketId = supportTicketId;
        OrderItemId = orderItemId;
        PartId = partId;
        Quantity = quantity;
        Note = note;
    }

    private SupportTicketDetailReplica() : base()
    {
    }

    public static SupportTicketDetailReplica Create(
        Guid id,
        Guid supportTicketId,
        Guid orderItemId,
        Guid? partId,
        int quantity,
        string? note = null)
    {
        return new SupportTicketDetailReplica(
            id,
            supportTicketId,
            orderItemId,
            partId,
            quantity,
            note);
    }
}
