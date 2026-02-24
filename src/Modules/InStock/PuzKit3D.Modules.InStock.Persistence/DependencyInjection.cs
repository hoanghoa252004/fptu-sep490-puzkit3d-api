using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.InStock.Application.Data;
using PuzKit3D.Modules.InStock.Domain.Repositories;
using PuzKit3D.Modules.InStock.Persistence.Repositories;

namespace PuzKit3D.Modules.InStock.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInStockPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<InStockDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IInStockUnitOfWork>(sp => sp.GetRequiredService<InStockDbContext>());

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
