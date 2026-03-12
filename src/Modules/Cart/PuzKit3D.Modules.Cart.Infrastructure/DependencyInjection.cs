using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Contract.InStock.InstockInventories;
using PuzKit3D.Contract.InStock.InstockPrices;
using PuzKit3D.Contract.InStock.InstockProductPriceDetails;
using PuzKit3D.Contract.InStock.InstockProducts;
using PuzKit3D.Contract.InStock.InstockProductVariants;
using PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockInventories;
using PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockPrices;
using PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockProductPriceDetails;
using PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockProducts;
using PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCartInfrastructure(
        this IServiceCollection services)
    {
        // Register Integration Event Handlers for InStock events
        
        // InstockProduct events
        services.AddScoped<IIntegrationEventHandler<InstockProductCreatedIntegrationEvent>, 
            InstockProductCreatedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<InstockProductUpdatedIntegrationEvent>, 
            InstockProductUpdatedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<InstockProductDeletedIntegrationEvent>, 
            InstockProductDeletedIntegrationEventHandler>();
        
        // InstockProductVariant events
        services.AddScoped<IIntegrationEventHandler<InstockProductVariantCreatedIntegrationEvent>, 
            InstockProductVariantCreatedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<InstockProductVariantUpdatedIntegrationEvent>, 
            InstockProductVariantUpdatedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<InstockProductVariantDeletedIntegrationEvent>, 
            InstockProductVariantDeletedIntegrationEventHandler>();
        
        // InstockInventory events
        services.AddScoped<IIntegrationEventHandler<InstockInventoryCreatedIntegrationEvent>, 
            InstockInventoryCreatedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<InstockInventoryUpdatedIntegrationEvent>, 
            InstockInventoryUpdatedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<InstockInventoryDeletedIntegrationEvent>, 
            InstockInventoryDeletedIntegrationEventHandler>();
        
        // InstockPrice events
        services.AddScoped<IIntegrationEventHandler<InstockPriceCreatedIntegrationEvent>, 
            InstockPriceCreatedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<InstockPriceUpdatedIntegrationEvent>, 
            InstockPriceUpdatedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<InstockPriceDeletedIntegrationEvent>, 
            InstockPriceDeletedIntegrationEventHandler>();
        
        // InstockProductPriceDetail events
        services.AddScoped<IIntegrationEventHandler<InstockProductPriceDetailCreatedIntegrationEvent>, 
            InstockProductPriceDetailCreatedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<InstockProductPriceDetailUpdatedIntegrationEvent>, 
            InstockProductPriceDetailUpdatedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<InstockProductPriceDetailDeletedIntegrationEvent>, 
            InstockProductPriceDetailDeletedIntegrationEventHandler>();

        return services;
    }
}
