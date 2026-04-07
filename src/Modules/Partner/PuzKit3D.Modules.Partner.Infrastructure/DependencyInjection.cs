using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Contract.Delivery;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Contract.User;
using PuzKit3D.Modules.Partner.Infrastructure.IntegrationEventHandlers.DeliveryTrackings;
using PuzKit3D.Modules.Partner.Infrastructure.IntegrationEventHandlers.PartnerProductOrders;
using PuzKit3D.Modules.Partner.Infrastructure.IntegrationEventHandlers.Payments;
using PuzKit3D.Modules.Partner.Infrastructure.IntegrationEventHandlers.Users;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Partner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPartnerInfrastructure(
        this IServiceCollection services)
    {
        //// User events
        services.AddScoped<IIntegrationEventHandler<UserEmailConfirmedIntegrationEvent>,
            UserEmailConfirmedIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<UserUpdatedIntegrationEvent>,
            UserUpdatedIntegrationEventHandler>();

        // Delivery tracking events
        services.AddScoped<IIntegrationEventHandler<OrderDeliveredIntegrationEvent>,
            OrderDeliveredIntegrationEventHandler>();

        // Payment events
        services.AddScoped<IIntegrationEventHandler<OrderExpiredToDoPaymentIntegrationEvent>,
            OrderExpiredToDoPaymentIntegrationEventHandler>();

        // Partner product order events
        services.AddScoped<IIntegrationEventHandler<PartnerProductOrderPaidSuccessIntegrationEvent>,
            PartnerProductOrderPaidSuccessIntegrationEventHandler>();
        return services;
    }
}
