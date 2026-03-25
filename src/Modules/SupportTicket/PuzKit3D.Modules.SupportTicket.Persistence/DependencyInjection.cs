using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.SupportTicket.Application.Repositories;
using PuzKit3D.Modules.SupportTicket.Application.Services;
using PuzKit3D.Modules.SupportTicket.Application.UnitOfWork;
using PuzKit3D.Modules.SupportTicket.Persistence.Repositories;
using PuzKit3D.Modules.SupportTicket.Persistence.Services;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.Modules.SupportTicket.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddSupportTicketPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SupportTicketDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", Schema.SupportTicket);
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

        services.AddScoped<ISupportTicketUnitOfWork>(sp => sp.GetRequiredService<SupportTicketDbContext>());
        services.AddScoped<ISupportTicketRepository, SupportTicketRepository>();
        services.AddScoped<IOrderReplicaRepository, OrderReplicaRepository>();
        services.AddScoped<IOrderDetailReplicaRepository, OrderDetailReplicaRepository>();
        services.AddScoped<IPartReplicaRepository, PartReplicaRepository>();
        services.AddScoped<ISupportTicketCodeGenerator, SupportTicketCodeGenerator>();

        return services;
    }
}
