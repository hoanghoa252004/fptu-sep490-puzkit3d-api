using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.Catalog.Application.Data;
using PuzKit3D.Modules.Catalog.Domain.Repositories;
using PuzKit3D.Modules.Catalog.Persistence.Repositories;

namespace PuzKit3D.Modules.Catalog.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<CatalogDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICatalogUnitOfWork>(sp => sp.GetRequiredService<CatalogDbContext>());

        services.AddScoped<IAssemblyMethodRepository, AssemblyMethodRepository>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IMaterialRepository, MaterialRepository>();
        services.AddScoped<ICapabilityRepository, CapabilityRepository>();

        return services;
    }
}
