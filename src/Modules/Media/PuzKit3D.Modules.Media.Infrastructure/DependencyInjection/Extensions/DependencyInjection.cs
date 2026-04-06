using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            services.AddSingleton<IAmazonS3>(sp =>
                new AmazonS3Client(
                    awsCredentials,
                    Amazon.RegionEndpoint.APSoutheast1
                ));
        }
        else
        {
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonS3>();
        }

        services.Configure<S3Settings>
(configuration.GetSection(S3Settings.ConfigurationSection));

        // Đăng kí service:
        services.AddTransient<IMediaService, S3MediaService>();

        return services;
    }
}

