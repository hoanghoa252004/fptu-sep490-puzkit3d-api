using MediatR;
using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.CustomDesign.Application.UnitOfWork;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.RequirementCapabilityDetails;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Proposals;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Milestones;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Phases;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotations;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.MilestoneQuotationDetails;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Workflows;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.ProductQuotations;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.CustomDesign.Persistence;

public sealed class CustomDesignDbContext : DbContext, ICustomDesignUnitOfWork
{
    private readonly IPublisher _publisher;

    public CustomDesignDbContext(
        DbContextOptions<CustomDesignDbContext> options,
        IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<CustomDesignRequest> CustomDesignRequests => Set<CustomDesignRequest>();
    public DbSet<CustomDesignRequirement> CustomDesignRequirements => Set<CustomDesignRequirement>();
    public DbSet<RequirementCapabilityDetail> RequirementCapabilityDetails => Set<RequirementCapabilityDetail>();
    public DbSet<CustomDesignAsset> CustomDesignAssets => Set<CustomDesignAsset>();
    public DbSet<TopicReplica> TopicReplicas => Set<TopicReplica>();
    public DbSet<AssemblyMethodReplica> AssemblyMethodReplicas => Set<AssemblyMethodReplica>();
    public DbSet<MaterialReplica> MaterialReplicas => Set<MaterialReplica>();
    public DbSet<CapabilityReplica> CapabilityReplicas => Set<CapabilityReplica>();
    public DbSet<DriveReplica> DriveReplicas => Set<DriveReplica>();
    public DbSet<Proposal> Proposals => Set<Proposal>();
    public DbSet<Milestone> Milestones => Set<Milestone>();
    public DbSet<Phase> Phases => Set<Phase>();
    public DbSet<MilestoneQuotation> MilestoneQuotations => Set<MilestoneQuotation>();
    public DbSet<MilestoneQuotationDetail> MilestoneQuotationDetails => Set<MilestoneQuotationDetail>();
    public DbSet<Workflow> Workflows => Set<Workflow>();
    public DbSet<ProductQuotation> ProductQuotations => Set<ProductQuotation>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(Schema.CustomDesign);

        builder.ApplyConfigurationsFromAssembly(typeof(CustomDesignDbContext).Assembly);

        // Apply seed data
        Configurations.SeedData.CustomDesignSeedDataConfiguration.SeedCustomDesignMasterData(builder);
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

    Task ICustomDesignUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
