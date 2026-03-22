using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.CompletedOrderItemReplicas;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.SupportTicket.Persistence.Repositories;

internal sealed class CompletedOrderItemReplicaRepository : ICompletedOrderItemReplicaRepository
{
    private readonly SupportTicketDbContext _dbContext;

    public CompletedOrderItemReplicaRepository(SupportTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> AddAsync(
        CompletedOrderItemReplica itemReplica,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.CompletedOrderItemReplicas.AddAsync(itemReplica, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("CompletedOrderItemReplica.AddError", ex.Message));
        }
    }

    public async Task<Result> AddRangeAsync(
        List<CompletedOrderItemReplica> itemReplicas,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.CompletedOrderItemReplicas.AddRangeAsync(itemReplicas, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("CompletedOrderItemReplica.AddError", ex.Message));
        }
    }
}
