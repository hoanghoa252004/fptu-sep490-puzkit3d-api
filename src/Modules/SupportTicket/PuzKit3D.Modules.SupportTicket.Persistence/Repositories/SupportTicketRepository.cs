using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;
using SupportTicketEntity = PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.SupportTicket;

namespace PuzKit3D.Modules.SupportTicket.Persistence.Repositories;

internal sealed class SupportTicketRepository : ISupportTicketRepository
{
    private readonly SupportTicketDbContext _dbContext;

    public SupportTicketRepository(SupportTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResultT<SupportTicketEntity>> GetByIdAsync(
        SupportTicketId id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var ticket = await _dbContext.SupportTickets
                .Include(st => st.Details)
                .FirstOrDefaultAsync(st => st.Id == id, cancellationToken);

            if (ticket == null)
                return Result.Failure<SupportTicketEntity>(
                    Error.NotFound("SupportTicket.NotFound", $"Support ticket with ID {id.Value} not found"));

            return Result.Success(ticket);
        }
        catch (Exception ex)
        {
            return Result.Failure<SupportTicketEntity>(
                Error.Failure("SupportTicket.GetError", ex.Message));
        }
    }

    public async Task<ResultT<List<SupportTicketEntity>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var tickets = await _dbContext.SupportTickets
                .Include(st => st.Details)
                .ToListAsync(cancellationToken);

            return Result.Success(tickets);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<SupportTicketEntity>>(
                Error.Failure("SupportTicket.GetError", ex.Message));
        }
    }

    public async Task<ResultT<List<SupportTicketEntity>>> GetByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var tickets = await _dbContext.SupportTickets
                .Include(st => st.Details)
                .Where(st => st.UserId == userId)
                .ToListAsync(cancellationToken);

            return Result.Success(tickets);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<SupportTicketEntity>>(
                Error.Failure("SupportTicket.GetError", ex.Message));
        }
    }

    public async Task<ResultT<List<SupportTicketEntity>>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var tickets = await _dbContext.SupportTickets
                .Include(st => st.Details)
                .Where(st => st.OrderId == orderId)
                .ToListAsync(cancellationToken);

            return Result.Success(tickets);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<SupportTicketEntity>>(
                Error.Failure("SupportTicket.GetError", ex.Message));
        }
    }

    public async Task<ResultT<List<SupportTicketEntity>>> GetByStatusAsync(
        SupportTicketStatus status,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var tickets = await _dbContext.SupportTickets
                .Include(st => st.Details)
                .Where(st => st.Status == status)
                .ToListAsync(cancellationToken);

            return Result.Success(tickets);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<SupportTicketEntity>>(
                Error.Failure("SupportTicket.GetError", ex.Message));
        }
    }

    public async Task<Result> AddAsync(
        SupportTicketEntity supportTicket,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SupportTickets.AddAsync(supportTicket, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("SupportTicket.AddError", ex.Message));
        }
    }

    public async Task<Result> UpdateAsync(
        SupportTicketEntity supportTicket,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _dbContext.SupportTickets.Update(supportTicket);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("SupportTicket.UpdateError", ex.Message));
        }
    }

    public async Task<Result> DeleteAsync(
        SupportTicketId id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var ticket = await _dbContext.SupportTickets.FindAsync(new object[] { id }, cancellationToken);
            if (ticket == null)
                return Result.Failure(Error.NotFound("SupportTicket.NotFound", $"Support ticket with ID {id.Value} not found"));

            _dbContext.SupportTickets.Remove(ticket);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("SupportTicket.DeleteError", ex.Message));
        }
    }
}
