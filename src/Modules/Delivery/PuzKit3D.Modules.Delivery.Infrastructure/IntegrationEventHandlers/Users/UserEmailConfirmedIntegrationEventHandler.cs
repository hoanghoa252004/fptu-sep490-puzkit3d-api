using PuzKit3D.Contract.User;
using PuzKit3D.Modules.Delivery.Persistence;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.Users;

internal sealed class UserEmailConfirmedIntegrationEventHandler : IIntegrationEventHandler<UserEmailConfirmedIntegrationEvent>
{
    private readonly DeliveryDbContext _dbContext;

    public UserEmailConfirmedIntegrationEventHandler(DeliveryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(UserEmailConfirmedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var existingUser = await _dbContext.UserReplicas.FindAsync(new object[] { @event.UserId }, cancellationToken: cancellationToken);

        if (existingUser is not null)
        {
            existingUser.Update(
                @event.Email,
                @event.PasswordHash,
                @event.RoleId,
                @event.FullName,
                @event.DateOfBirth,
                @event.PhoneNumber,
                @event.Province,
                @event.District,
                @event.Ward,
                @event.StreetAddress);
        }
        else
        {
            var newUser = UserReplica.Create(
                @event.UserId,
                @event.Email,
                @event.PasswordHash,
                @event.RoleId,
                @event.FullName,
                @event.DateOfBirth,
                @event.PhoneNumber,
                @event.Province,
                @event.District,
                @event.Ward,
                @event.StreetAddress);

            await _dbContext.UserReplicas.AddAsync(newUser, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

