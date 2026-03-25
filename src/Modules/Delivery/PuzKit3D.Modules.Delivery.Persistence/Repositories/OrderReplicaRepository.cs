using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Persistence.Repositories;

internal sealed class OrderReplicaRepository : IOrderReplicaRepository
{
    private readonly DeliveryDbContext _dbContext;

    public OrderReplicaRepository(DeliveryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResultT<OrderReplica>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var replica = await _dbContext.OrderReplicas
                //.Include(r => r.OrderItems)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (replica == null)
                return Result.Failure<OrderReplica>(
                    Error.NotFound("OrderReplica.NotFound", $"Order replica with ID {id} not found"));

            return Result.Success(replica);
        }
        catch (Exception ex)
        {
            return Result.Failure<OrderReplica>(
                Error.Failure("OrderReplica.GetError", ex.Message));
        }
    }

    public async Task<ResultT<List<OrderReplica>>> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var replicas = await _dbContext.OrderReplicas
                //.Include(r => r.OrderItems)
                .Where(r => r.CustomerId == customerId)
                .ToListAsync(cancellationToken);

            return Result.Success(replicas);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<OrderReplica>>(
                Error.Failure("OrderReplica.GetError", ex.Message));
        }
    }

    public async Task<Result> AddAsync(
        OrderReplica replica,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.OrderReplicas.AddAsync(replica, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.Failure("OrderReplica.AddError", ex.Message));
        }
    }
}
