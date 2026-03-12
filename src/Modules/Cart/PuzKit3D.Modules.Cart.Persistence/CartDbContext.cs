using MediatR;
using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.Cart.Persistence;

public sealed class CartDbContext : DbContext, ICartUnitOfWork
{
    private readonly IPublisher _publisher;

    public CartDbContext(
        DbContextOptions<CartDbContext> options,
        IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<Domain.Entities.Carts.Cart> Carts => Set<Domain.Entities.Carts.Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<InStockProductReplica> InStockProductReplicas => Set<InStockProductReplica>();
    public DbSet<InStockProductVariantReplica> InStockProductVariantReplicas => Set<InStockProductVariantReplica>();
    public DbSet<InStockProductPriceDetailReplica> InStockProductPriceDetailReplicas => Set<InStockProductPriceDetailReplica>();
    public DbSet<InStockPriceReplica> InStockPriceReplicas => Set<InStockPriceReplica>();
    public DbSet<InStockInventoryReplica> InStockInventoryReplicas => Set<InStockInventoryReplica>();
    public DbSet<PartnerProductReplica> PartnerProductReplicas => Set<PartnerProductReplica>();
    public DbSet<UserReplica> UserReplicas => Set<UserReplica>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(Schema.Cart);

        builder.ApplyConfigurationsFromAssembly(typeof(CartDbContext).Assembly);

        // Apply seed data
        Configurations.SeedData.CartSeedDataConfiguration.SeedInStockPriceReplicas(builder);
        Configurations.SeedData.CartSeedDataConfiguration.SeedInStockProductReplicas(builder);
        Configurations.SeedData.CartSeedDataConfiguration.SeedInStockVariantsAndRelatedReplicas(builder);
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

                var domainEvents = GetDomainEvents();

                await DispatchDomainEventsAsync(domainEvents, cancellationToken);

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

    private async Task DispatchDomainEventsAsync(List<IDomainEvent> domainEvents, CancellationToken cancellationToken)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }
    }

    Task ICartUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
