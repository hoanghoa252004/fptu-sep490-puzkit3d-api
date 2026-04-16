using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Contract.SupportTicket.SupportTickets;

public sealed record ExchangeItem(
    Guid VariantId,
    int Quantity);

public sealed record SupportTicketExchangeProcessingIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid SupportTicketId,
    Guid OrderId,
    List<ExchangeItem> Items) : IIntegrationEvent;
