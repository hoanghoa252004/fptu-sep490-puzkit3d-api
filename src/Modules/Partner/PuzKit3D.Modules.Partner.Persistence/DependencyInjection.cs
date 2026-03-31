using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.Services;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Commands.CreatePartnerProductRequest;
using PuzKit3D.Modules.Partner.Persistence.Repositories;
using PuzKit3D.Modules.Partner.Persistence.Services;
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
        services.AddScoped<IPartnerRepository, PartnerRepository>();
        services.AddScoped<IImportServiceConfigRepository, ImportServiceConfigRepository>();
        services.AddScoped<IPartnerProductRepository, PartnerProductRepository>();
        services.AddScoped<IPartnerProductRequestRepository, PartnerProductRequestRepository>();
        services.AddScoped<IPartnerProductRequestDetailRepository, PartnerProductRequestDetailRepository>();

        // Register CodeGenerator services
        services.AddScoped<IPartnerProductRequestCodeGenerator, PartnerProductRequestCodeGenerator>();
        services.AddScoped<IPartnerProductQuotationCodeGenerator, PartnerProductQuotationCodeGenerator>();
        services.AddScoped<IPartnerProductOrderCodeGenerator, PartnerProductOrderCodeGenerator>();

        return services;
    }
}
