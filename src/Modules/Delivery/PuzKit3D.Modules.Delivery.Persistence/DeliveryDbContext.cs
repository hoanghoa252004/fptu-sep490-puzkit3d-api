using MediatR;
using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Delivery.Application.UnitOfWork;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.Delivery.Persistence;

/// <summary>
/// DbContext for Delivery Module
/// </summary>
public sealed class DeliveryDbContext : DbContext, IDeliveryUnitOfWork
{
    private readonly IPublisher _publisher;

    public DeliveryDbContext(
        DbContextOptions<DeliveryDbContext> options,
        IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<DeliveryTracking> DeliveryTrackings => Set<DeliveryTracking>();

    public DbSet<DeliveryTrackingDetail> DeliveryTrackingDetails => Set<DeliveryTrackingDetail>();

    public DbSet<OrderReplica> OrderReplicas => Set<OrderReplica>();
    public DbSet<OrderDetailReplica> OrderDetailReplicas => Set<OrderDetailReplica>();

    public DbSet<SupportTicketReplica> SupportTicketReplicas => Set<SupportTicketReplica>();
    public DbSet<SupportTicketDetailReplica> SupportTicketDetailReplicas => Set<SupportTicketDetailReplica>();

    public DbSet<UserReplica> UserReplicas => Set<UserReplica>();
    public DbSet<DriveReplica> DriveReplicas => Set<DriveReplica>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(Schema.Delivery);

        builder.ApplyConfigurationsFromAssembly(typeof(DeliveryDbContext).Assembly);

        // Apply seed data
        Configurations.SeedData.DeliverySeedDataConfiguration.SeedDeliveryMasterData(builder);
    }

    public async Task<T> ExecuteAsync<T>(
        Func<Task<T>> action,
        CancellationToken cancellationToken = default)
    {
        var strategy = Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
            {
                var response = await action();

                // Check if result is failure and rollback
                if (response is Result result && result.IsFailure)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return response;
                }

                // Dispatch domain events
                do
                {
                    var domainEvents = GetDomainEvents();
                    if (domainEvents.Any())
                    {
                        await DispatchDomainEventsAsync(domainEvents, cancellationToken);
                    }
                } while (CheckDomainEventRemain());

                // Save changes and commit
                await SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return response;
            }
        });
    }

    /// <summary>
    /// Get all unpublished domain events
    /// </summary>
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

    /// <summary>
    /// Check if there are remaining domain events to publish
    /// </summary>
    private bool CheckDomainEventRemain()
    {
        var domainEventEntities = ChangeTracker.Entries()
            .Select(e => e.Entity)
            .OfType<IEntity>()
            .Where(e => e.DomainEvents.Any())
            .ToList();

        return domainEventEntities.Any(entity => entity.DomainEvents.Any());
    }

    /// <summary>
    /// Dispatch domain events using MediatR
    /// </summary>
    private async Task DispatchDomainEventsAsync(
        List<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }
    }

    Task IDeliveryUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
