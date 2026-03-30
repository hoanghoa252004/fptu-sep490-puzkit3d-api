using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Services;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Persistence.Repositories;
using PuzKit3D.Modules.InStock.Persistence.Services;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.InStock.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInStockPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<InStockDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", Schema.Instock);
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            })
            .UseSnakeCaseNamingConvention();

            if (configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"))
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<IInStockUnitOfWork>(sp => sp.GetRequiredService<InStockDbContext>());

        services.AddScoped<IInstockProductRepository, InstockProductRepository>();
        services.AddScoped<IInstockProductVariantRepository, InstockProductVariantRepository>();
        services.AddScoped<IInstockInventoryRepository, InstockInventoryRepository>();
        services.AddScoped<IInstockPriceRepository, InstockPriceRepository>();
        services.AddScoped<IInstockProductPriceDetailRepository, InstockProductPriceDetailRepository>();
        services.AddScoped<IInstockOrderRepository, InstockOrderRepository>();
        services.AddScoped<IInstockOrderConfigRepository, InstockOrderConfigRepository>();

        services.AddScoped<ITopicReplicaRepository, TopicReplicaRepository>();
        services.AddScoped<IAssemblyMethodReplicaRepository, AssemblyMethodReplicaRepository>();
        services.AddScoped<ICapabilityReplicaRepository, CapabilityReplicaRepository>();
        services.AddScoped<IMaterialReplicaRepository, MaterialReplicaRepository>();

        services.AddScoped<IInstockProductCodeGenerator, InstockProductCodeGenerator>();
        services.AddScoped<IPartCodeGenerator, PartCodeGenerator>();
        services.AddScoped<IInstockProductVariantSkuGenerator, InstockProductVariantSkuGenerator>();
        services.AddScoped<IInstockOrderCodeGenerator, InstockOrderCodeGenerator>();
        services.AddScoped<ISupportTicketReplicaRepository, SupportTicketReplicaRepository>();

        return services;
    }
}
