using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Persistence.Repositories;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.CustomDesign.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomDesignPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CustomDesignDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", Schema.CustomDesign);
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

        services.AddScoped<ITopicReplicaRepository, TopicReplicaRepository>();
        services.AddScoped<IAssemblyMethodReplicaRepository, AssemblyMethodReplicaRepository>();
        services.AddScoped<IMaterialReplicaRepository, MaterialReplicaRepository>();
        services.AddScoped<ICapabilityReplicaRepository, CapabilityReplicaRepository>();

        return services;
    }
}
