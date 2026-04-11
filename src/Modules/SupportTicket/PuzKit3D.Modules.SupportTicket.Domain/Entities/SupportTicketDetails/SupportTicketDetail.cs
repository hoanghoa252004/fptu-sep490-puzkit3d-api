using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTicketDetails;

public sealed class SupportTicketDetail : Entity<SupportTicketDetailId>
{
    public SupportTicketId SupportTicketId { get; private set; }
    public Guid OrderItemId { get; private set; }
    public Guid? DriveId { get; private set; }
    public int Quantity { get; private set; }
    public string? Note { get; private set; }

    private SupportTicketDetail(
        SupportTicketDetailId id,
        SupportTicketId supportTicketId,
        Guid orderItemId,
        Guid? driveId,
        int quantity,
        string? note) : base(id)
    {
        SupportTicketId = supportTicketId;
        OrderItemId = orderItemId;
        DriveId = driveId;
        Quantity = quantity;
        Note = note;
    }

    private SupportTicketDetail() : base()
    {
    }

    public static ResultT<SupportTicketDetail> Create(
        SupportTicketId supportTicketId,
        Guid orderItemId,
        Guid? driveId,
        int quantity,
        string? note = null)
    {
        if (orderItemId == Guid.Empty)
            return Result.Failure<SupportTicketDetail>(SupportTicketDetailError.InvalidOrderItemId());

        //if (partId.HasValue == false || partId == Guid.Empty)
        //    return Result.Failure<SupportTicketDetail>(SupportTicketDetailError.InvalidPartId());

        if (quantity <= 0)
            return Result.Failure<SupportTicketDetail>(SupportTicketDetailError.InvalidQuantity());

        if (!string.IsNullOrWhiteSpace(note) && note.Length > 500)
            return Result.Failure<SupportTicketDetail>(SupportTicketDetailError.NoteTooLong());

        var detail = new SupportTicketDetail(
            SupportTicketDetailId.Create(),
            supportTicketId,
            orderItemId,
            driveId,
            quantity,
            note);

        return Result.Success(detail);
    }

    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity > 0)
            Quantity = newQuantity;
    }

    public void UpdateNote(string? newNote)
    {
        if (newNote == null || (newNote.Length <= 500))
            Note = newNote;
    }
}
