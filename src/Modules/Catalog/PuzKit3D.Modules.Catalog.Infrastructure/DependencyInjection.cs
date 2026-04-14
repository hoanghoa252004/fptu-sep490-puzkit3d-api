using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Modules.Catalog.Infrastructure.IntegrationEventHandlers.SupportTickets;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Register Integration Event Handlers - Support Ticket Events
        services.AddScoped<IIntegrationEventHandler<SupportTicketReplaceDriveProcessingIntegrationEvent>,
            SupportTicketReplaceDriveProcessingIntegrationEventHandler>();

        return services;
    }
}
