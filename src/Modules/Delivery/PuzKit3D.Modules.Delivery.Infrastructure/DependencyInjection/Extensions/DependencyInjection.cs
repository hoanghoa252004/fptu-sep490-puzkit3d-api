using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PuzKit3D.Contract.Catalog.Drives;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Contract.User;
using PuzKit3D.Modules.Delivery.Application;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Infrastructure.DependencyInjection.Options;
using PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.Drives;
using PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.InstockOrders;
using PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.PartnerProductOrders;
using PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.SupportTickets;
using PuzKit3D.Modules.Delivery.Infrastructure.IntegrationEventHandlers.Users;
using PuzKit3D.Modules.Delivery.Infrastructure.Services;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Delivery.Infrastructure.DependencyInjection.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDeliveryInfrastructure(
        this IServiceCollection services, IConfiguration configuration, IHostEnvironment _env)
    {
        // Cấu hình DeliverySettings từ appsettings
        services.Configure<DeliverySettings>(configuration.GetSection(DeliverySettings.ConfigurationSection));

        services.Configure<DeliveryApplicationSettings>(configuration.GetSection(DeliveryApplicationSettings.ConfigurationSection));

        // Đăng ký HttpClient cho GHN service
        services.AddHttpClient<IDeliveryService, GhnDeliveryService>();

        // Đăng kí service:
        services.AddScoped<IDeliveryService, GhnDeliveryService>();

        //// InstockOrder events
        services.AddScoped<IIntegrationEventHandler<InstockOrderCreatedIntegrationEvent>,
            InstockOrderCreatedIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<InstockOrderStatusChangedIntegrationEvent>,
            InstockOrderStatusChangedIntegrationEventHandler>();

        //// PartnerProductOrder events
        services.AddScoped<IIntegrationEventHandler<PartnerProductOrderCreatedIntegrationEvent>,
            PartnerProductOrderCreatedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<PartnerProductOrderStatusUpdatedIntegrationEvent>,
            PartnerProductOrderStatusUpdatedIntegrationEventHandler>();

        //// Support Ticket events
        services.AddScoped<IIntegrationEventHandler<SupportTicketCreatedIntegrationEvent>,
            SupportTicketCreatedIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<SupportTicketDeletedIntegrationEvent>,
            SupportTicketDeletedIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<SupportTicketStatusChangedIntegrationEvent>,
            SupportTicketStatusChangedIntegrationEventHandler>();


        //// User events
        services.AddScoped<IIntegrationEventHandler<UserEmailConfirmedIntegrationEvent>,
            UserEmailConfirmedIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<UserUpdatedIntegrationEvent>,
            UserUpdatedIntegrationEventHandler>();

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

