using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Infrastructure.DependencyInjection.Options;
using PuzKit3D.Modules.Delivery.Infrastructure.Services;

namespace PuzKit3D.Modules.Delivery.Infrastructure.DependencyInjection.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDeliveryInfrastructure(
        this IServiceCollection services, IConfiguration configuration, IHostEnvironment _env)
    {
        // Cấu hình DeliverySettings từ appsettings
        services.Configure<DeliverySettings>(configuration.GetSection(DeliverySettings.ConfigurationSection));

        // Đăng ký HttpClient cho GHN service
        services.AddHttpClient<IDeliveryService, GhnDeliveryService>();

        // Đăng kí service:
        services.AddScoped<IDeliveryService, GhnDeliveryService>();

        return services;
    }
}

