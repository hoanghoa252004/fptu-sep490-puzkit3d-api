using MediatR;
using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Wallet.Application.UnitOfWork;
using PuzKit3D.Modules.Wallet.Domain.Entities.WalletConfigs;
using PuzKit3D.Modules.Wallet.Domain.Entities.Wallets;
using PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.SharedKernel.Infrastructure.Data;
using WalletEntity = PuzKit3D.Modules.Wallet.Domain.Entities.Wallets.Wallet;
using WalletTransactionEntity = PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions.WalletTransaction;

namespace PuzKit3D.Modules.Wallet.Persistence;

public sealed class WalletDbContext : DbContext, IWalletUnitOfWork
{
    private readonly IPublisher _publisher;

    public WalletDbContext(
        DbContextOptions<WalletDbContext> options,
        IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<WalletEntity> Wallets => Set<WalletEntity>();
    public DbSet<WalletTransactionEntity> WalletTransactions => Set<WalletTransactionEntity>();
    public DbSet<WalletConfig> WalletConfigs => Set<WalletConfig>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(Schema.Wallet);

        builder.ApplyConfigurationsFromAssembly(typeof(WalletDbContext).Assembly);

        SeedWalletConfigs(builder);
    }

    private static void SeedWalletConfigs(ModelBuilder builder)
    {
        builder.Entity<WalletConfig>().HasData(
            new
            {
                Id = new Guid("00000000-0000-0000-0000-000000000003"),
                OnlineOrderReturnPercentage = 80m,
                OnlineOrderCompletedRewardPercentage = 5m,
                CODOrderCompletedRewardPercentage = 2m,
                UpdatedAt = new DateTime(2026, 3, 30, 0, 0, 0, DateTimeKind.Utc)
            });
    }

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default)
    {
        var strategy = Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
            {
                var response = await action();
                if (response is Result result && result.IsFailure)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return response;
                }
                do
                {
                    var domainEvents = GetDomainEvents();
                    if (domainEvents.Any())
                    {
                        await DispatchDomainEventsAsync(domainEvents, cancellationToken);
                    }
                } while (CheckDomainEventRemain());

                await SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return response;
            }
        });
    }

    private List<IDomainEvent> GetDomainEvents()
    {
        var domainEventEntities = ChangeTracker.Entries()
            .Select(e => e.Entity)
            .OfType<IEntity>()
            .Where(e => e.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEventEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        domainEventEntities.ForEach(e => e.ClearDomainEvents());

        return domainEvents;
    }

    private bool CheckDomainEventRemain()
    {
        return ChangeTracker.Entries()
            .Select(e => e.Entity)
            .OfType<IEntity>()
            .Any(e => e.DomainEvents.Any());
    }

    private async Task DispatchDomainEventsAsync(List<IDomainEvent> domainEvents, CancellationToken cancellationToken)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }
    }

    async Task IWalletUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }
}
