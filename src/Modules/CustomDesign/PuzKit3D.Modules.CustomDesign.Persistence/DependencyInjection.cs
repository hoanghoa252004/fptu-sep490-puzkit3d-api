using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Application.Services;
using PuzKit3D.Modules.CustomDesign.Application.UnitOfWork;
using PuzKit3D.Modules.CustomDesign.Persistence.Repositories;
using PuzKit3D.Modules.CustomDesign.Persistence.Services;
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

        services.AddScoped<ICustomDesignUnitOfWork>(sp => sp.GetRequiredService<CustomDesignDbContext>());

        services.AddScoped<ICustomDesignRequirementRepository, CustomDesignRequirementRepository>();
        services.AddScoped<ICustomDesignRequestRepository, CustomDesignRequestRepository>();
        services.AddScoped<ICustomDesignAssetRepository, CustomDesignAssetRepository>();
        services.AddScoped<IRequirementCapabilityDetailRepository, RequirementCapabilityDetailRepository>();

        services.AddScoped<ITopicReplicaRepository, TopicReplicaRepository>();
        services.AddScoped<IAssemblyMethodReplicaRepository, AssemblyMethodReplicaRepository>();
        services.AddScoped<IMaterialReplicaRepository, MaterialReplicaRepository>();
        services.AddScoped<ICapabilityReplicaRepository, CapabilityReplicaRepository>();

        services.AddScoped<IProposalRepository, ProposalRepository>();
        services.AddScoped<IMilestoneRepository, MilestoneRepository>();
        services.AddScoped<IPhaseRepository, PhaseRepository>();
        services.AddScoped<IMilestoneQuotationRepository, MilestoneQuotationRepository>();
        services.AddScoped<IMilestoneQuotationDetailRepository, MilestoneQuotationDetailRepository>();
        services.AddScoped<IWorkflowRepository, WorkflowRepository>();
        services.AddScoped<IProductQuotationRepository, ProductQuotationRepository>();

        // Services
        services.AddScoped<ICustomDesignRequirementCodeGenerator, CustomDesignRequirementCodeGenerator>();
        services.AddScoped<ICustomDesignRequestCodeGenerator, CustomDesignRequestCodeGenerator>();
        services.AddScoped<ICustomDesignAssetCodeGenerator, CustomDesignAssetCodeGenerator>();
        return services;
    }
}
