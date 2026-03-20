using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Contract.InStock.InstockProducts;
using PuzKit3D.Modules.Feedback.Infrastructure.IntegrationEventHandlers.InstockOrders;
using PuzKit3D.Modules.Feedback.Infrastructure.IntegrationEventHandlers.InstockProducts;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Feedback.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFeedbackInfrastructure(
        this IServiceCollection services)
    {
        // InstockOrder events
        services.AddScoped<IIntegrationEventHandler<InstockOrderCompletedIntegrationEvent>,
            InstockOrderCompletedIntegrationEventHandler>();

        // InstockProduct events
        services.AddScoped<IIntegrationEventHandler<InstockProductCreatedIntegrationEvent>,
            InstockProductCreatedIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<InstockProductUpdatedIntegrationEvent>,
            InstockProductUpdatedIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<InstockProductDeletedIntegrationEvent>,
            InstockProductDeletedIntegrationEventHandler>();

        return services;
    }
}
