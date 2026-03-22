using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.Feedback.Application.Repositories;
using PuzKit3D.Modules.Feedback.Application.UnitOfWork;
using PuzKit3D.Modules.Feedback.Persistence.Repositories;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.Feedback.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddFeedbackPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<FeedbackDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", Schema.Feedback);
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

        services.AddScoped<IFeedbackUnitOfWork>(sp => sp.GetRequiredService<FeedbackDbContext>());
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();
        services.AddScoped<IOrderReplicaRepository, OrderReplicaRepository>();
        services.AddScoped<IOrderDetailReplicaRepository, OrderDetailReplicaRepository>();
        services.AddScoped<IProductReplicaRepository, ProductReplicaRepository>();
        
        return services;
    }
}
