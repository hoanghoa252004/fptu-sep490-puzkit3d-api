using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.Payment.Application.Abstractions;
using PuzKit3D.Modules.Payment.Infrastructure.PaymentGateways.VnPay;
using VNPAY.Extensions;

namespace PuzKit3D.Modules.Payment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var vnpayConfig = configuration.GetSection("VNPAY");

        services.AddVnpayClient(config =>
        {
            config.TmnCode = vnpayConfig["TmnCode"]!;
            config.HashSecret = vnpayConfig["HashSecret"]!;
            config.CallbackUrl = vnpayConfig["CallbackUrl"]!;
            config.BaseUrl = vnpayConfig["BaseUrl"]!;
            config.Version = vnpayConfig["Version"]!;
            config.OrderType = vnpayConfig["OrderType"]!;
        });

        services.AddScoped<IPaymentGateway, VNPAYGateway>();

        return services;
    }
}
