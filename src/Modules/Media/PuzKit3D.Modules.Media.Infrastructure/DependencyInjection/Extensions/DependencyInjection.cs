using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Modules.Media.Application.Services;
using PuzKit3D.Modules.Media.Infrastructure.DependencyInjection.Options;
using PuzKit3D.Modules.Media.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Media.Infrastructure.DependencyInjection.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddMediaInfrastructure(
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

        services.AddAWSService<IAmazonS3>();

        services.Configure<S3Settings>
            (configuration.GetSection(S3Settings.ConfigurationSection));

        // Đăng kí service:
        services.AddScoped<IMediaService, S3MediaService>();

        return services;
    }
}

