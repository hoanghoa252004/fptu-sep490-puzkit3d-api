using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Contract.Feedback;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Contract.Partner.PartnerProductOrders;
using PuzKit3D.Contract.User;
using PuzKit3D.Contract.Wallet;
using PuzKit3D.Modules.Wallet.Infrastructure.IntegrationEventHandlers.Feedback;
using PuzKit3D.Modules.Wallet.Infrastructure.IntegrationEventHandlers.InStock;
using PuzKit3D.Modules.Wallet.Infrastructure.IntegrationEventHandlers.Orders;
using PuzKit3D.Modules.Wallet.Infrastructure.IntegrationEventHandlers.ProductPartner;
using PuzKit3D.Modules.Wallet.Infrastructure.IntegrationEventHandlers.User;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Wallet.Infrastructure.DependencyInjection.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddWalletInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register integration event handlers
        services.AddScoped<IIntegrationEventHandler<UserEmailConfirmedIntegrationEvent>,
            UserEmailConfirmedIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<FeedbackCreatedWithHighestRatingIntegrationEvent>,
            FeedbackCreatedWithHighestRatingIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<OrderCancelledRefundCoinIntegrationEvent>,
            OrderCancelledRefundCoinIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<OrderReturnRefundCoinIntegrationEvent>,
            OrderReturnRefundCoinIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<CoinUsedIntegrationEvent>,
            CoinUsedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<PartnerProductCoinUsedIntegrationEvent>,
            PartnerProductCoinUsedIntegrationEventHandler>();
        
        services.AddScoped<IIntegrationEventHandler<PartnerProductOrderRefundedIntegrationEvent>,
            PartnerProductOrderRefundedIntegrationEventHandler>();

        return services;
    }
}
