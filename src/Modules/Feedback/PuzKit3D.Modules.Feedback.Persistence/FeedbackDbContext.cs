using MediatR;
using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Feedback.Application.UnitOfWork;
using PuzKit3D.Modules.Feedback.Persistence.Configurations.SeedData;
using FeedbackEntity = PuzKit3D.Modules.Feedback.Domain.Entities.Feedbacks.Feedback;
using PuzKit3D.Modules.Feedback.Domain.Entities.OrderReplicas;
using PuzKit3D.Modules.Feedback.Domain.Entities.ProductReplicas;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.Feedback.Persistence;

public sealed class FeedbackDbContext : DbContext, IFeedbackUnitOfWork
{
    private readonly IPublisher _publisher;

    public FeedbackDbContext(
        DbContextOptions<FeedbackDbContext> options,
        IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<FeedbackEntity> Feedbacks => Set<FeedbackEntity>();
    public DbSet<CompletedOrderReplica> CompletedOrderReplicas => Set<CompletedOrderReplica>();
    public DbSet<ProductReplica> ProductReplicas => Set<ProductReplica>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(Schema.Feedback);

        builder.ApplyConfigurationsFromAssembly(typeof(FeedbackDbContext).Assembly);

        // Seed data
        builder.SeedProductReplicas();
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

                await base.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return response;
            }
        });
    }

    async Task IFeedbackUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
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

    private async Task DispatchDomainEventsAsync(List<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }
    }
}
