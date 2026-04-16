using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Contract.Catalog.AssemblyMethods;
using PuzKit3D.Contract.Catalog.Capabilities;
using PuzKit3D.Contract.Catalog.Drives;
using PuzKit3D.Contract.Catalog.Materials;
using PuzKit3D.Contract.Catalog.Topics;
using PuzKit3D.Modules.CustomDesign.Infrastructure.IntegrationEventHandlers.Catalog.AssemblyMethods;
using PuzKit3D.Modules.CustomDesign.Infrastructure.IntegrationEventHandlers.Catalog.Capabilities;
using PuzKit3D.Modules.CustomDesign.Infrastructure.IntegrationEventHandlers.Catalog.Drives;
using PuzKit3D.Modules.CustomDesign.Infrastructure.IntegrationEventHandlers.Catalog.Materials;
using PuzKit3D.Modules.CustomDesign.Infrastructure.IntegrationEventHandlers.Catalog.Topics;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.CustomDesign.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomDesignInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Register Integration Event Handlers - Catalog Topic Events
        services.AddScoped<IIntegrationEventHandler<TopicCreatedIntegrationEvent>,
            TopicCreatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<TopicUpdatedIntegrationEvent>,
            TopicUpdatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<TopicDeletedIntegrationEvent>,
            TopicDeletedIntegrationEventHandler>();

        // Register Integration Event Handlers - Catalog AssemblyMethod Events
        services.AddScoped<IIntegrationEventHandler<AssemblyMethodCreatedIntegrationEvent>,
            AssemblyMethodCreatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<AssemblyMethodUpdatedIntegrationEvent>,
            AssemblyMethodUpdatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<AssemblyMethodDeletedIntegrationEvent>,
            AssemblyMethodDeletedIntegrationEventHandler>();

        // Register Integration Event Handlers - Catalog Material Events
        services.AddScoped<IIntegrationEventHandler<MaterialCreatedIntegrationEvent>,
            MaterialCreatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<MaterialUpdatedIntegrationEvent>,
            MaterialUpdatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<MaterialDeletedIntegrationEvent>,
            MaterialDeletedIntegrationEventHandler>();

        // Register Integration Event Handlers - Catalog Capability Events
        services.AddScoped<IIntegrationEventHandler<CapabilityCreatedIntegrationEvent>,
            CapabilityCreatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<CapabilityUpdatedIntegrationEvent>,
            CapabilityUpdatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<CapabilityDeletedIntegrationEvent>,
            CapabilityDeletedIntegrationEventHandler>();

        // Register Integration Event Handlers - Catalog Drive Events
        services.AddScoped<IIntegrationEventHandler<DriveCreatedIntegrationEvent>,
            DriveCreatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<DriveUpdatedIntegrationEvent>,
            DriveUpdatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<DriveDeletedIntegrationEvent>,
            DriveDeletedIntegrationEventHandler>();
        return services;
    }
}
