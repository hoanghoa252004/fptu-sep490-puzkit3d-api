using Amazon.SimpleEmail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Contract.User;
using PuzKit3D.Modules.Notification.Application.Services;
using PuzKit3D.Modules.Notification.Infrastructure.DependencyInjection.Options;
using PuzKit3D.Modules.Notification.Infrastructure.IntegrationEventHandlers;
using PuzKit3D.Modules.Notification.Infrastructure.Services;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Notification.Infrastructure.DependencyInjection.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationInfrastructure(
        this IServiceCollection services, IConfiguration configuration, IHostEnvironment _env)
    {
        // ==========  Setting DI for Aws Ses========== 
        var awsOptions = configuration.GetAWSOptions();
        if (_env.IsDevelopment())
        {
            // Lấy config từ appsettings
            awsOptions.Credentials = new Amazon.Runtime.BasicAWSCredentials(
            configuration["AWS:AccessKey"],
            configuration["AWS:SecretKey"]
            );
        }

        services.AddDefaultAWSOptions(awsOptions);

        services.AddAWSService<IAmazonSimpleEmailService>();

        services.Configure<EmailSettings>
            (configuration.GetSection(EmailSettings.ConfigurationSection));

        // Đăng kí service:
        //services.AddScoped<AwsSesEmailService>();
        services.AddScoped<IEmailService, AwsSesEmailService>();

        services.AddScoped<IIntegrationEventHandler<UserRegisteredIntegrationEvent>,
            UserRegisteredIntegrationEventHandler>();
        return services;
    }
}
