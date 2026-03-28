using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
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
            var chain = new CredentialProfileStoreChain();

            if (!chain.TryGetAWSCredentials("default", out var awsCredentials))
            {
                throw new Exception("Cannot load AWS credentials");
            }

            services.AddDefaultAWSOptions(awsOptions);

            services.AddSingleton<IAmazonSimpleEmailService>(sp =>
                new AmazonSimpleEmailServiceClient(
                    awsCredentials,
                    Amazon.RegionEndpoint.APSoutheast1
                ));
        }
        else
        {
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonSimpleEmailService>();
        }

        services.Configure<EmailSettings>
            (configuration.GetSection(EmailSettings.ConfigurationSection));

        // Đăng kí service:
        //services.AddScoped<AwsSesEmailService>();
        services.AddScoped<IEmailService, AwsSesEmailService>();

        services.AddScoped<IIntegrationEventHandler<UserRegisteredIntegrationEvent>,
            UserRegisteredIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<ForgotPasswordIntegrationEvent>,
            ForgotPasswordIntegrationEventHandler>();
        return services;
    }
}
