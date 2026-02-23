using Microsoft.OpenApi.Models;

namespace PuzKit3D.WebApi.DependencyInjection.Extensions;

internal static class SwaggerExtensions
{
    internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PuzKit3D API",
                Version = "v1",
                Description = "PuzKit3D - 3D Puzzle Model Kit API"
            });
        });

        return services;
    }
}