using MediatR;
using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockInventories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrderDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductCapabilityDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.Modules.InStock.Domain.Entities.Parts;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;
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

    public DbSet<InstockProduct> InstockProducts => Set<InstockProduct>();
    public DbSet<InstockProductVariant> InstockProductVariants => Set<InstockProductVariant>();
    public DbSet<InstockProductCapabilityDetail> InstockProductCapabilityDetails => Set<InstockProductCapabilityDetail>();
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<InstockInventory> InstockInventories => Set<InstockInventory>();
    public DbSet<InstockPrice> InstockPrices => Set<InstockPrice>();
    public DbSet<InstockProductPriceDetail> InstockProductPriceDetails => Set<InstockProductPriceDetail>();
    public DbSet<InstockOrder> InstockOrders => Set<InstockOrder>();
    public DbSet<InstockOrderDetail> InstockOrderDetails => Set<InstockOrderDetail>();
    public DbSet<AssemblyMethodReplica> AssemblyMethodReplicas => Set<AssemblyMethodReplica>();
    public DbSet<TopicReplica> TopicReplicas => Set<TopicReplica>();
    public DbSet<MaterialReplica> MaterialReplicas => Set<MaterialReplica>();
    public DbSet<CapabilityReplica> CapabilityReplicas => Set<CapabilityReplica>();

    public DbSet<SupportTicketReplica> SupportTicketReplicas => Set<SupportTicketReplica>();
    public DbSet<SupportTicketDetailReplica> SupportTicketDetailReplicas => Set<SupportTicketDetailReplica>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(Schema.Instock);

        builder.ApplyConfigurationsFromAssembly(typeof(InStockDbContext).Assembly);

        // Apply seed data
        Configurations.SeedData.InstockSeedDataConfiguration.SeedReplicas(builder);
        Configurations.SeedData.InstockSeedDataConfiguration.SeedPrices(builder);
        Configurations.SeedData.InstockSeedDataConfiguration.SeedProducts(builder);
        Configurations.SeedData.InstockSeedDataConfiguration.SeedVariants(builder);
        Configurations.SeedData.InstockSeedDataConfiguration.SeedProductCapabilityDetails(builder);
        Configurations.SeedData.InstockSeedDataConfiguration.SeedParts(builder);
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

    Task IInStockUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}

