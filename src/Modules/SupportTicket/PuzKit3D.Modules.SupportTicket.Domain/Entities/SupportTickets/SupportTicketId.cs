namespace PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;

public sealed class SupportTicketId
{
    public Guid Value { get; }

    private SupportTicketId(Guid value)
    {
        Value = value;
    }

    public static SupportTicketId Create()
        => new(Guid.NewGuid());

    public static SupportTicketId From(Guid id)
        => new(id);

    public override bool Equals(object? obj)
        => obj is SupportTicketId other && Value == other.Value;

    public override int GetHashCode()
        => Value.GetHashCode();

    public override string ToString()
        => Value.ToString();
}
