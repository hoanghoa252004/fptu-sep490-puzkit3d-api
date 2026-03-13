using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.InStock.Infrastructure.IntegrationEventHandlers.InstockOrders;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.InStock.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInStockInfrastructure(
        this IServiceCollection services)
    {
        // Register Integration Event Handlers
        services.AddScoped<IIntegrationEventHandler<InstockOrderPaidSuccessIntegrationEvent>,
            InstockOrderPaidSuccessIntegrationEventHandler>();

        return services;
    }
}
