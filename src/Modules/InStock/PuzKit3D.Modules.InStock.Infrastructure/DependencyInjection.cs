using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Contract.Catalog.AssemblyMethods;
using PuzKit3D.Contract.Catalog.Capabilities;
using PuzKit3D.Contract.Catalog.Materials;
using PuzKit3D.Contract.Catalog.Topics;
using PuzKit3D.Contract.Delivery;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.InStock.Application.Services;
using PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.Catalog.AssemblyMethods;
using PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.Catalog.Capabilities;
using PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.Catalog.Materials;
using PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.Catalog.Topics;
using PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.DeliveryTrackings;
using PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.InstockOrders;
using PuzKit3D.Modules.InStock.Infrastructure.Options;
using PuzKit3D.Modules.InStock.Infrastructure.Services;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInStockInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Configure S3Settings
        services.Configure<S3Settings>(configuration.GetSection(S3Settings.ConfigurationSection));

        // Register Services
        services.AddScoped<IAssetUrlService, AssetUrlService>();

        // Register Integration Event Handlers - InStock Events
        services.AddScoped<IIntegrationEventHandler<InstockOrderPaidSuccessIntegrationEvent>,
            InstockOrderPaidSuccessIntegrationEventHandler>();

        // Register Integration Event Handlers - Delivery Events
        services.AddScoped<IIntegrationEventHandler<OrderDeliveredIntegrationEvent>,
            OrderDeliveredIntegrationEventHandler>();

        // Register Integration Event Handlers - Catalog AssemblyMethod Events
        services.AddScoped<IIntegrationEventHandler<AssemblyMethodCreatedIntegrationEvent>,
            AssemblyMethodCreatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<AssemblyMethodUpdatedIntegrationEvent>,
            AssemblyMethodUpdatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<AssemblyMethodDeletedIntegrationEvent>,
            AssemblyMethodDeletedIntegrationEventHandler>();

        // Register Integration Event Handlers - Catalog Topic Events
        services.AddScoped<IIntegrationEventHandler<TopicCreatedIntegrationEvent>,
            TopicCreatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<TopicUpdatedIntegrationEvent>,
            TopicUpdatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<TopicDeletedIntegrationEvent>,
            TopicDeletedIntegrationEventHandler>();

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

        return services;
    }
}





