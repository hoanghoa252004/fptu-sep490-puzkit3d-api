using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.SupportTicket.SupportTickets;

public sealed record ReplaceDriveItem(
    Guid DriveId,
    int Quantity);

public sealed record SupportTicketReplaceDriveProcessingIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid SupportTicketId,
    Guid OrderId,
    List<ReplaceDriveItem> Items) : IIntegrationEvent;
