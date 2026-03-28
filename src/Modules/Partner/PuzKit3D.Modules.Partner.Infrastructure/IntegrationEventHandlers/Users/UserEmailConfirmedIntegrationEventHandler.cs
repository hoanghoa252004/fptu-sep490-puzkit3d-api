using PuzKit3D.Contract.User;
using PuzKit3D.Modules.Partner.Domain.Entities.Replicas;
using PuzKit3D.Modules.Partner.Persistence;
using PuzKit3D.SharedKernel.Application.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Partner.Infrastructure.IntegrationEventHandlers.Users;

internal sealed class UserEmailConfirmedIntegrationEventHandler : IIntegrationEventHandler<UserEmailConfirmedIntegrationEvent>
{
    private readonly PartnerDbContext _dbContext;

    public UserEmailConfirmedIntegrationEventHandler(PartnerDbContext dbContext)
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