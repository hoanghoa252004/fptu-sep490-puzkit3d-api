using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.Partner.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPartnerPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PartnerDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", Schema.Partner);
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

        services.AddScoped<IPartnerUnitOfWork>(sp => sp.GetRequiredService<PartnerDbContext>());

        return services;
    }
}
