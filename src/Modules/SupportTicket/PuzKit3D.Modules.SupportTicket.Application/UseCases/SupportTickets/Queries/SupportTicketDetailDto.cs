namespace PuzKit3D.Modules.SupportTicket.Application.UseCases.SupportTickets.Queries;

public sealed record SupportTicketDetailDto(
    Guid Id,
    Guid OrderDetailId,
    Guid? DriveId,
    int Quantity,
    string? Note);
