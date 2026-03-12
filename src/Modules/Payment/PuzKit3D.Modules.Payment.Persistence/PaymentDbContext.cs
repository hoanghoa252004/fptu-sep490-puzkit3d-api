using MediatR;
using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Payment.Application.UnitOfWork;
using PuzKit3D.Modules.Payment.Domain.Entities.OrderReplicas;
using PuzKit3D.Modules.Payment.Domain.Entities.Transactions;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.Payment.Persistence;

public sealed class PaymentDbContext : DbContext, IPaymentUnitOfWork
{
    private readonly IPublisher _publisher;

    public PaymentDbContext(
        DbContextOptions<PaymentDbContext> options,
        IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<Domain.Entities.Payments.Payment> Payments => Set<Domain.Entities.Payments.Payment>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<OrderReplica> OrderReplicas => Set<OrderReplica>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(Schema.Payment);

        builder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
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
        var domainEventEntities = ChangeTracker.Entries()
            .Select(e => e.Entity)
            .OfType<IEntity>()
            .Where(e => e.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEventEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        return domainEvents.Any();
    }

    private async Task DispatchDomainEventsAsync(List<IDomainEvent> domainEvents, CancellationToken cancellationToken)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }
    }

    Task IPaymentUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
