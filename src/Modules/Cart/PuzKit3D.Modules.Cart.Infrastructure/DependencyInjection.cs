using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Contract.InStock.InstockInventories;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Contract.InStock.InstockPrices;
using PuzKit3D.Contract.InStock.InstockProductPriceDetails;
using PuzKit3D.Contract.InStock.InstockProducts;
using PuzKit3D.Contract.InStock.InstockProductVariants;
using PuzKit3D.Contract.Partner.PartnerProducts;
using PuzKit3D.Contract.Partner.Partners;
using PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockInventories;
using PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockOrders;
using PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockPrices;
using PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockProductPriceDetails;
using PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockProducts;
using PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockProductVariants;
using PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.Partner.PartnerProducts;
using PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.Partner.Partners;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCartInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {

        // Register Integration Event Handlers for InStock events
        services.AddScoped<IIntegrationEventHandler<InstockProductCreatedIntegrationEvent>,
            InstockProductCreatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<InstockProductUpdatedIntegrationEvent>,
            InstockProductUpdatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<InstockProductDeletedIntegrationEvent>,
            InstockProductDeletedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<InstockProductVariantCreatedIntegrationEvent>,
            InstockProductVariantCreatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<InstockProductVariantUpdatedIntegrationEvent>,
            InstockProductVariantUpdatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<InstockProductVariantDeletedIntegrationEvent>,
            InstockProductVariantDeletedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<InstockInventoryCreatedIntegrationEvent>,
            InstockInventoryCreatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<InstockInventoryUpdatedIntegrationEvent>,
            InstockInventoryUpdatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<InstockInventoryDeletedIntegrationEvent>,
            InstockInventoryDeletedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<InstockPriceCreatedIntegrationEvent>,
            InstockPriceCreatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<InstockPriceUpdatedIntegrationEvent>,
            InstockPriceUpdatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<InstockPriceDeletedIntegrationEvent>,
            InstockPriceDeletedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<InstockProductPriceDetailCreatedIntegrationEvent>,
            InstockProductPriceDetailCreatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<InstockProductPriceDetailUpdatedIntegrationEvent>,
            InstockProductPriceDetailUpdatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<InstockProductPriceDetailDeletedIntegrationEvent>,
            InstockProductPriceDetailDeletedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<InstockOrderCreatedIntegrationEvent>,
            InstockOrderCreatedIntegrationEventHandler>();

        // Register Integration Event Handlers for Partner events
        services.AddScoped<IIntegrationEventHandler<PartnerProductCreatedIntegrationEvent>,
            PartnerProductCreatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<PartnerProductUpdatedIntegrationEvent>,
            PartnerProductUpdatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<PartnerProductDeletedIntegrationEvent>,
            PartnerProductDeletedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<PartnerProductActivatedIntegrationEvent>,
            PartnerProductActivatedIntegrationEventHandler>();
        services.AddScoped<IIntegrationEventHandler<PartnerDeletedIntegrationEvent>,
            PartnerDeletedIntegrationEventHandler>();

        return services;
    }
}
