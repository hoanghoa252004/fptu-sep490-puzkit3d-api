using Amazon.SimpleEmail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.Notification.Application.Services;
using PuzKit3D.Modules.Notification.Infrastructure.DependencyInjection.Options;
using PuzKit3D.Modules.Notification.Infrastructure.Services;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Notification.Infrastructure.DependencyInjection.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddNotificationInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        // ==========  Setting DI for Aws Ses========== 
        // Lấy config từ appsettings

        var awsOptions = configuration.GetAWSOptions();
        awsOptions.Credentials = new Amazon.Runtime.BasicAWSCredentials(
        configuration["AWS:AccessKey"],
        configuration["AWS:SecretKey"]
        );

        services.AddDefaultAWSOptions(awsOptions);

        services.AddAWSService<IAmazonSimpleEmailService>();

        services.Configure<EmailSettings>
            (configuration.GetSection(EmailSettings.ConfigurationSection));

        // Đăng kí service:
        services.AddScoped<IEmailService, AwsSesEmailService>();

        return services;
    }
}
