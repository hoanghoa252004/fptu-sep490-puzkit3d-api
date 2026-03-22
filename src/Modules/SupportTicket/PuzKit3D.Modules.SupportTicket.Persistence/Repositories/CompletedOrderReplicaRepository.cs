using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.CompletedOrderReplicas;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Persistence.Repositories;

internal sealed class CompletedOrderReplicaRepository : ICompletedOrderReplicaRepository
{
    private readonly SupportTicketDbContext _dbContext;

    public CompletedOrderReplicaRepository(SupportTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResultT<CompletedOrderReplica>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var replica = await _dbContext.CompletedOrderReplicas
                .Include(r => r.OrderItems)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (replica == null)
                return Result.Failure<CompletedOrderReplica>(
                    Error.NotFound("CompletedOrderReplica.NotFound", $"Completed order replica with ID {id} not found"));

            return Result.Success(replica);
        }
        catch (Exception ex)
        {
            return Result.Failure<CompletedOrderReplica>(
                Error.Failure("CompletedOrderReplica.GetError", ex.Message));
        }
    }

    public async Task<ResultT<List<CompletedOrderReplica>>> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var replicas = await _dbContext.CompletedOrderReplicas
                .Include(r => r.OrderItems)
                .Where(r => r.CustomerId == customerId)
                .ToListAsync(cancellationToken);

            return Result.Success(replicas);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<CompletedOrderReplica>>(
                Error.Failure("CompletedOrderReplica.GetError", ex.Message));
        }
    }

    public async Task<Result> AddAsync(
        CompletedOrderReplica replica,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.CompletedOrderReplicas.AddAsync(replica, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("CompletedOrderReplica.AddError", ex.Message));
        }
    }
}
