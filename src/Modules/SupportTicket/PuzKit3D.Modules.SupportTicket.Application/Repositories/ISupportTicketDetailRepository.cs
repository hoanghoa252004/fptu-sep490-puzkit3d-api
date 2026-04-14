using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTicketDetails;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Application.Repositories;

public interface ISupportTicketDetailRepository
{
    Task<ResultT<List<SupportTicketDetail>>> GetBySupportTicketIdAsync(
        Guid supportTicketId,
        CancellationToken cancellationToken = default);
}
