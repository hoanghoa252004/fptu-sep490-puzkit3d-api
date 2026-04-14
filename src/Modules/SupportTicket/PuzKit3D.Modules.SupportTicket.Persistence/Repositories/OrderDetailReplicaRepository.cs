using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.OrderReplicas;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Linq;

namespace PuzKit3D.Modules.SupportTicket.Persistence.Repositories;

internal sealed class OrderDetailReplicaRepository : IOrderDetailReplicaRepository
{
    private readonly SupportTicketDbContext _dbContext;

    public OrderDetailReplicaRepository(SupportTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OrderDetailReplica?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.OrderDetailReplicas
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Result> AddAsync(
        OrderDetailReplica itemReplica,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.OrderDetailReplicas.AddAsync(itemReplica, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("OrderDetailReplica.AddError", ex.Message));
        }
    }

    public async Task<Result> AddRangeAsync(
        List<OrderDetailReplica> itemReplicas,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.OrderDetailReplicas.AddRangeAsync(itemReplicas, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("OrderDetailReplica.AddError", ex.Message));
        }
    }

    public async Task<List<OrderDetailReplica>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.OrderDetailReplicas
            .Where(o => o.OrderId == orderId)
            .ToListAsync(cancellationToken);
    }
}
