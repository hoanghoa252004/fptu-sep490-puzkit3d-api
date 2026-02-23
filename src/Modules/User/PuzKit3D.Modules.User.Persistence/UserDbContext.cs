using Microsoft.EntityFrameworkCore;
using PuzKit3D.SharedKernel.Application.Clock;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.User.Persistence;

/// <summary>
/// DbContext for User domain entities with schema: user
/// </summary>
public sealed class UserDbContext : BaseDbContext
{
    public UserDbContext(
        DbContextOptions<UserDbContext> options,
        IDateTimeProvider dateTimeProvider) : base(options, dateTimeProvider)
    {
    }

    public override Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        throw new NotImplementedException();
    }

    // Add your User domain entities DbSets here
    // Example: public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("user");

        // Configure your User domain entities here
        // Example: builder.Entity<UserProfile>().ToTable("user_profile");
    }

    protected override Task PublishDomainEventAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
