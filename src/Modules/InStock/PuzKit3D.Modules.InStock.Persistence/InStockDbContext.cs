using MediatR;
using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.Data;
using PuzKit3D.Modules.InStock.Domain.Entities.Orders;
using PuzKit3D.Modules.InStock.Domain.Entities.Products;
using PuzKit3D.SharedKernel.Application.Clock;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.InStock.Persistence;

public sealed class InStockDbContext : DbContext, IInStockUnitOfWork
{
    private readonly IPublisher _publisher;

    public InStockDbContext(
        DbContextOptions<InStockDbContext> options,
        IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("instock");

        builder.ApplyConfigurationsFromAssembly(typeof(InStockDbContext).Assembly);
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

                await SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                await DispatchDomainEventsAsync(cancellationToken);

                return response;
            }
        });
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEventEntities = ChangeTracker.Entries<Entity<object>>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEventEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        domainEventEntities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }
    }

    Task IInStockUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
