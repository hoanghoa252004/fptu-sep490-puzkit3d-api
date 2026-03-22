namespace PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTicketDetails;

public sealed class SupportTicketDetailId
{
    public Guid Value { get; }

    private SupportTicketDetailId(Guid value)
    {
        Value = value;
    }

    public static SupportTicketDetailId Create()
        => new(Guid.NewGuid());

    public static SupportTicketDetailId From(Guid id)
        => new(id);

    public override bool Equals(object? obj)
        => obj is SupportTicketDetailId other && Value == other.Value;

    public override int GetHashCode()
        => Value.GetHashCode();

    public override string ToString()
        => Value.ToString();
}
