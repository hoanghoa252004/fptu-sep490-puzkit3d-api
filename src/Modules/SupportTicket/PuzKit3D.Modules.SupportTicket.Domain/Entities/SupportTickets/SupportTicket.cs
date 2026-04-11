using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTicketDetails;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.DomainEvents;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;

public sealed class SupportTicket : AggregateRoot<SupportTicketId>
{
    private readonly List<SupportTicketDetail> _details = new();

    public Guid UserId { get; private set; }
    public Guid OrderId { get; private set; }
    public SupportTicketType Type { get; private set; }
    public SupportTicketStatus Status { get; private set; }
    public string Reason { get; private set; } = null!;
    public string Proof { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string Code { get; private set; }
    public IReadOnlyCollection<SupportTicketDetail> Details => _details.AsReadOnly();

    private SupportTicket(
        SupportTicketId id,
        Guid userId,
        Guid orderId,
        SupportTicketType type,
        SupportTicketStatus status,
        string reason,
        string proof,
        DateTime createdAt,
        string code) : base(id)
    {
        UserId = userId;
        OrderId = orderId;
        Type = type;
        Status = status;
        Reason = reason;
        Proof = proof;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
        Code = code;
    }

    private SupportTicket() : base()
    {
    }

    public static ResultT<SupportTicket> Create(
        Guid userId,
        Guid orderId,
        SupportTicketType type,
        string reason,
        string proof,
        string code,
        DateTime? createdAt = null)
    {
        if (userId == Guid.Empty)
            return Result.Failure<SupportTicket>(SupportTicketError.InvalidUserId());

        if (orderId == Guid.Empty)
            return Result.Failure<SupportTicket>(SupportTicketError.InvalidOrderId());

        if (string.IsNullOrWhiteSpace(reason))
            return Result.Failure<SupportTicket>(SupportTicketError.InvalidReason());

        if (reason.Length > 1000)
            return Result.Failure<SupportTicket>(SupportTicketError.ReasonTooLong());

        if (string.IsNullOrWhiteSpace(proof))
            return Result.Failure<SupportTicket>(SupportTicketError.InvalidProof());

        if (proof.Length > 500)
            return Result.Failure<SupportTicket>(SupportTicketError.ProofTooLong());

        var now = createdAt ?? DateTime.UtcNow;

        var ticket = new SupportTicket(
            SupportTicketId.Create(),
            userId,
            orderId,
            type,
            SupportTicketStatus.Open,
            reason,
            proof,
            now,
            code);

        return Result.Success(ticket);
    }

    public void RaiseCreateSupportTicket()
    {
        // Build detail information from _details list
        var detailInfos = _details.Select(d => new SupportTicketDetailInfo(
            d.Id.Value,
            d.OrderItemId,
            d.DriveId,
            d.Quantity,
            d.Note)).ToList();

        var now = DateTime.UtcNow;

        // Emit SupportTicketCreatedDomainEvent
        var @event = new SupportTicketCreatedDomainEvent(
            Id.Value,
            Code,
            UserId,
            OrderId,
            Type.ToString(),
            SupportTicketStatus.Open.ToString(),
            Reason,
            Proof,
            now,
            now,
            detailInfos);

        RaiseDomainEvent(@event);
    }

    public ResultT<bool> UpdateStatus(SupportTicketStatus newStatus)
    {
        // Validate status transition
        var validTransition = (Status, newStatus) switch
        {
            (SupportTicketStatus.Open, SupportTicketStatus.Processing) => true,
            (SupportTicketStatus.Open, SupportTicketStatus.Rejected) => true,
            (SupportTicketStatus.Processing, SupportTicketStatus.Resolved) => true,
            _ => false
        };

        if (!validTransition)
            return Result.Failure<bool>(SupportTicketError.InvalidStatusTransition(Status, newStatus));

        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;

        // Emit SupportTicketStatusChangedDomainEvent
        var @event = new SupportTicketStatusChangedDomainEvent(
            Id.Value,
            Status.ToString(),
            UpdatedAt);

        RaiseDomainEvent(@event);

        return Result.Success(true);
    }

    public void AddDetail(SupportTicketDetail detail)
    {
        if (!_details.Contains(detail))
        {
            _details.Add(detail);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void RemoveDetail(SupportTicketDetail detail)
    {
        if (_details.Contains(detail))
        {
            _details.Remove(detail);
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void Delete()
    {
        // Emit SupportTicketDeletedDomainEvent
        var @event = new SupportTicketDeletedDomainEvent(Id.Value);
        RaiseDomainEvent(@event);
    }
}
