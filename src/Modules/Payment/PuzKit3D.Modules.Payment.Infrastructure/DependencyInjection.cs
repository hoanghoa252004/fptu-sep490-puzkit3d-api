using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.Payment.Application.Abstractions;
using PuzKit3D.Modules.Payment.Infrastructure.IntegrationEventHandlers.InstockOrders;
using PuzKit3D.Modules.Payment.Infrastructure.PaymentGateways;
using PuzKit3D.Modules.Payment.Infrastructure.PaymentGateways.VNPAY;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Payment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<VNPAYGateway>();
        services.AddScoped<IVnPaySignatureValidator, VnPaySignatureValidator>();

        services.AddScoped<IPaymentGatewayFactory, PaymentGatewayFactory>();

        // InstockOrder events
        services.AddScoped<IIntegrationEventHandler<InstockOrderCreatedIntegrationEvent>,
            InstockOrderCreatedIntegrationEventHandler>();

        return services;
    }
}
