using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            config.BaseUrl = vnpayConfig["BaseUrl"]!; // Tùy chọn. Nếu không thiết lập, giá trị mặc định là URL thanh toán môi trường TEST
            config.Version = vnpayConfig["Version"]!; // Tùy chọn. Nếu không thiết lập, giá trị mặc định là "2.1.0"
            config.OrderType = vnpayConfig["OrderType"]!; // Tùy chọn. Nếu không thiết lập, giá trị mặc định là "other"
        });

        return services;
    }
}
