using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.Modules.Cart.Persistence.Repositories;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.Cart.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddCartPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CartDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", Schema.Cart);
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            });

            // Enable sensitive data logging in development
            if (configuration.GetValue<bool>("Logging:EnableSensitiveDataLogging"))
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<ICartUnitOfWork>(sp => sp.GetRequiredService<CartDbContext>());

        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartQueryRepository, CartQueryRepository>();

        return services;
    }
}
