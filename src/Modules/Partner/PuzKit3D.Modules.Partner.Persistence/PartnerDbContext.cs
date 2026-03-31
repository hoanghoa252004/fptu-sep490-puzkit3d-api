using MediatR;
using Microsoft.EntityFrameworkCore;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotationDetails;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequestDetails;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.Modules.Partner.Domain.Entities.Replicas;
using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.Partner.Persistence;

public sealed class PartnerDbContext : DbContext, IPartnerUnitOfWork
{
    private readonly IPublisher _publisher;

    public PartnerDbContext(
        DbContextOptions<PartnerDbContext> options,
        IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<ImportServiceConfig> ImportServiceConfigs => Set<ImportServiceConfig>();
    public DbSet<Domain.Entities.Partners.Partner> Partners => Set<Domain.Entities.Partners.Partner>();
    public DbSet<PartnerProduct> PartnerProducts => Set<PartnerProduct>();
    public DbSet<PartnerProductRequest> PartnerProductRequests => Set<PartnerProductRequest>();
    public DbSet<PartnerProductRequestDetail> PartnerProductRequestDetails => Set<PartnerProductRequestDetail>();
    public DbSet<PartnerProductQuotation> PartnerProductQuotations => Set<PartnerProductQuotation>();
    public DbSet<PartnerProductQuotationDetail> PartnerProductQuotationDetails => Set<PartnerProductQuotationDetail>();
    public DbSet<PartnerProductOrder> PartnerProductOrders => Set<PartnerProductOrder>();
    public DbSet<PartnerProductOrderDetail> PartnerProductOrderDetails => Set<PartnerProductOrderDetail>();
    public DbSet<UserReplica> UserReplicas => Set<UserReplica>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(Schema.Partner);

        builder.ApplyConfigurationsFromAssembly(typeof(PartnerDbContext).Assembly);
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

                // 👇 DEBUG TRƯỚC KHI SAVE
                foreach (var entry in ChangeTracker.Entries())
                {
                    Console.WriteLine($"{entry.Entity.GetType().Name} - {entry.State}");
                }

                // 👇 DEBUG GIÁ TRỊ (optional nhưng rất hữu ích)
                foreach (var entry in ChangeTracker.Entries())
                {
                    foreach (var prop in entry.Properties)
                    {
                        Console.WriteLine($"{entry.Entity.GetType().Name}.{prop.Metadata.Name} = {prop.CurrentValue}");
                    }
                }

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

    Task IPartnerUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
