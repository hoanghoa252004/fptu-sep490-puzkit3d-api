using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Delivery.Persistence.Repositories;

internal sealed class UserReplicaRepository : IUserReplicaRepository
{
    private readonly DeliveryDbContext _context;

    public UserReplicaRepository(DeliveryDbContext context)
    {
        _context = context;
    }

    public async Task<ResultT<UserReplica>> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _context.UserReplicas
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserReplica>(
                Error.NotFound("User.NotFound", $"User with ID {userId} not found"));
        }

        return Result.Success(user);
    }
}
