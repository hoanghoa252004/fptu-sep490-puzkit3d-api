using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.SharedKernel.Domain.Results;
using SupportTicketEntity = PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.SupportTicket;

namespace PuzKit3D.Modules.SupportTicket.Application.Repositories;

public interface ISupportTicketRepository
{
    Task<ResultT<SupportTicketEntity>> GetByIdAsync(SupportTicketId id, CancellationToken cancellationToken = default);
    Task<ResultT<List<SupportTicketEntity>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ResultT<List<SupportTicketEntity>>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ResultT<List<SupportTicketEntity>>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<ResultT<List<SupportTicketEntity>>> GetByStatusAsync(SupportTicketStatus status, CancellationToken cancellationToken = default);
    Task<Result> AddAsync(SupportTicketEntity supportTicket, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(SupportTicketEntity supportTicket, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(SupportTicketId id, CancellationToken cancellationToken = default);
}
