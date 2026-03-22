using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.SupportTicket.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSupportTicketInfrastructure(
        this IServiceCollection services)
    {
        //// InstockOrder events
        //services.AddScoped<IIntegrationEventHandler<InstockOrderCompletedIntegrationEvent>,
        //    InstockOrderCompletedIntegrationEventHandler>();

        return services;
    }
}
