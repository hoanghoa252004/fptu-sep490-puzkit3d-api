using MediatR;
using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;
using PuzKit3D.Modules.Catalog.Domain.Entities.Formulas;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityDrives;
using PuzKit3D.Modules.Catalog.Domain.Entities.TopicMaterialCapabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityMaterialAssemblies;
using PuzKit3D.Modules.Catalog.Persistence.Configurations.SeedData;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.Catalog.Persistence;

public sealed class CatalogDbContext : DbContext, ICatalogUnitOfWork
{
    private readonly IPublisher _publisher;

    public CatalogDbContext(
        DbContextOptions<CatalogDbContext> options,
        IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<AssemblyMethod> AssemblyMethods => Set<AssemblyMethod>();
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<Capability> Capabilities => Set<Capability>();
    public DbSet<Drive> Drives => Set<Drive>();
    public DbSet<Formula> Formulas => Set<Formula>();
    public DbSet<FormulaValueValidation> FormulaValueValidations => Set<FormulaValueValidation>();
    public DbSet<CapabilityDrive> CapabilityDrives => Set<CapabilityDrive>();
    public DbSet<TopicMaterialCapability> TopicMaterialCapabilities => Set<TopicMaterialCapability>();
    public DbSet<CapabilityMaterialAssembly> CapabilityMaterialAssemblies => Set<CapabilityMaterialAssembly>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(Schema.Catalog);

        builder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        
        builder.SeedCatalogMasterData();
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

    Task ICatalogUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
