using PuzKit3D.SharedKernel.Application.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Contract.SupportTicket.SupportTickets;

public sealed record SupportTicketTypeResendResolvedIntegrationEvent(
    Guid Id,
    DateTime OccurredOn,
    Guid CustomerId,
    Guid OrderId,
    decimal GrandTotalAmount) : IIntegrationEvent;
