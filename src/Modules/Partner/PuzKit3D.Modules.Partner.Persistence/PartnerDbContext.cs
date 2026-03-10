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

    Task IPartnerUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
