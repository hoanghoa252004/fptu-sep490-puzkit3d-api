using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Persistence.Repositories;

namespace PuzKit3D.Modules.Cart.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddCartPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<CartDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "cart");
            })
            .UseSnakeCaseNamingConvention());

        services.AddScoped<ICartUnitOfWork>(sp => sp.GetRequiredService<CartDbContext>());

        services.AddScoped<ICartRepository, CartRepository>();

        return services;
    }
}
