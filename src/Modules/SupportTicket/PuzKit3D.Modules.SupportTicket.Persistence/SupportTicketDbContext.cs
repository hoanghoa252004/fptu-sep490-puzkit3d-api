using MediatR;
using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.SupportTicket.Application.UnitOfWork;
using PuzKit3D.Modules.SupportTicket.Persistence.Configurations;
using SupportTicketEntity = PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.SupportTicket;
using SupportTicketDetailEntity = PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTicketDetails.SupportTicketDetail;
using OrderReplicaEntity = PuzKit3D.Modules.SupportTicket.Domain.Entities.OrderReplicas.OrderReplica;
using OrderDetailReplicaEntity = PuzKit3D.Modules.SupportTicket.Domain.Entities.OrderReplicas.OrderDetailReplica;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.SharedKernel.Infrastructure.Data;
using PuzKit3D.Modules.SupportTicket.Domain.Entities;

namespace PuzKit3D.Modules.SupportTicket.Persistence;

public sealed class SupportTicketDbContext : DbContext, ISupportTicketUnitOfWork
{
    private readonly IPublisher _publisher;

    public SupportTicketDbContext(
        DbContextOptions<SupportTicketDbContext> options,
        IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<SupportTicketEntity> SupportTickets => Set<SupportTicketEntity>();
    public DbSet<SupportTicketDetailEntity> SupportTicketDetails => Set<SupportTicketDetailEntity>();
    public DbSet<OrderReplicaEntity> OrderReplicas => Set<OrderReplicaEntity>();
    public DbSet<OrderDetailReplicaEntity> OrderDetailReplicas => Set<OrderDetailReplicaEntity>();
    public DbSet<DriveReplica> DriveReplicas => Set<DriveReplica>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema(Schema.SupportTicket);
        builder.ApplyConfigurationsFromAssembly(typeof(SupportTicketConfiguration).Assembly);

        // Apply seed data
        //Configurations.SeedData.SupportTicketSeedDataConfiguration.SeedPartReplicas(builder);

        base.OnModelCreating(builder);
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

    Task ISupportTicketUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
