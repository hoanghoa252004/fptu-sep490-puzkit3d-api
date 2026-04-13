using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Persistence.Repositories;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.Catalog.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", Schema.Catalog);
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            })
            .UseSnakeCaseNamingConvention();

            // Enable sensitive data logging in development
            if (configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"))
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<ICatalogUnitOfWork>(sp => sp.GetRequiredService<CatalogDbContext>());

        services.AddScoped<IAssemblyMethodRepository, AssemblyMethodRepository>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IMaterialRepository, MaterialRepository>();
        services.AddScoped<ICapabilityRepository, CapabilityRepository>();
        services.AddScoped<IDriveRepository, DriveRepository>();
        services.AddScoped<IFormulaRepository, FormulaRepository>();
        services.AddScoped<IFormulaValueValidationRepository, FormulaValueValidationRepository>();
        services.AddScoped<ICapabilityDriveRepository, CapabilityDriveRepository>();
        services.AddScoped<ITopicMaterialCapabilityRepository, TopicMaterialCapabilityRepository>();
        services.AddScoped<ICapabilityMaterialAssemblyRepository, CapabilityMaterialAssemblyRepository>();

        return services;
    }
}
