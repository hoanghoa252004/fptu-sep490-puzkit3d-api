using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.Wallet.Application.Repositories;
using PuzKit3D.Modules.Wallet.Application.UnitOfWork;
using PuzKit3D.Modules.Wallet.Persistence.Repositories;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.Wallet.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddWalletPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<WalletDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", Schema.Wallet);
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

        services.AddScoped<IWalletUnitOfWork>(sp => sp.GetRequiredService<WalletDbContext>());
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();

        return services;
    }
}
