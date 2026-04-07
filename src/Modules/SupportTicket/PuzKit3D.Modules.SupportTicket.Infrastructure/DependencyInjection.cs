using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Contract.InStock.Part;
using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Modules.SupportTicket.Infrastructure.IntegrationEventHandlers.InstockOrders;
using PuzKit3D.Modules.SupportTicket.Infrastructure.IntegrationEventHandlers.PartnerProductOrders;
using PuzKit3D.Modules.SupportTicket.Infrastructure.IntegrationEventHandlers.Parts;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.SupportTicket.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSupportTicketInfrastructure(
        this IServiceCollection services)
    {
        //// InstockOrder events
        services.AddScoped<IIntegrationEventHandler<InstockOrderCreatedIntegrationEvent>,
            InstockOrderCreatedIntegrationEventHandler>();

        //// Part events
        services.AddScoped<IIntegrationEventHandler<PartCreatedIntegrationEvent>,
            PartCreatedIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<PartUpdatedIntegrationEvent>,
            PartUpdatedIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<PartDeletedIntegrationEvent>,
            PartDeletedIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<InstockOrderStatusChangedIntegrationEvent>,
            InstockOrderStatusChangedIntegrationEventHandler>();

        //// PartnerProductOrder events
        services.AddScoped<IIntegrationEventHandler<PartnerProductOrderCreatedIntegrationEvent>,
            PartnerProductOrderCreatedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<PartnerProductOrderStatusUpdatedIntegrationEvent>,
            PartnerProductOrderStatusUpdatedIntegrationEventHandler>();
        return services;
    }
}
