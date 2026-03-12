using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Contract.InStock;
using PuzKit3D.Modules.Cart.Infrastructure.IntegrationEventHandlers.InStock;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Cart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCartInfrastructure(
        this IServiceCollection services)
    {
        // Register Integration Event Handlers for InStock events
        services.AddScoped<IIntegrationEventHandler<InstockProductVariantCreatedIntegrationEvent>, 
            InstockProductVariantCreatedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<InstockProductVariantUpdatedIntegrationEvent>, 
            InstockProductVariantUpdatedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<InstockInventoryChangedIntegrationEvent>, 
            InstockInventoryChangedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<InstockPriceChangedIntegrationEvent>, 
            InstockPriceChangedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<InstockProductPriceDetailChangedIntegrationEvent>, 
            InstockProductPriceDetailChangedIntegrationEventHandler>();

        return services;
    }
}
