using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Presentation.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Presentation.DependencyInjection.Extensions;

public static class ServiceCollectionExtentions
{
    /// <summary>
    /// Register all IEndpoint implementations from Presentation layer
    /// </summary>
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        // Find all types that implement IEndpoint
        var endpointTypes = assembly.GetTypes()
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t)
                     && t is { IsClass: true, IsAbstract: false });

        // Register each endpoint
        foreach (var type in endpointTypes)
        {
            services.AddSingleton(typeof(IEndpoint), type);
        }

        return services;
    }
}