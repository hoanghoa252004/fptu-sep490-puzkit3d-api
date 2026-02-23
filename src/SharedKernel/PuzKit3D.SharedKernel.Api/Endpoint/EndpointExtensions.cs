using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Api.Endpoint;

public static class EndpointExtensions
{
    /// <summary>
    /// Map all registered endpoints to the application
    /// </summary>
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        // Get all registered IEndpoint implementations from DI
        var endpoints = app.ServiceProvider.GetServices<IEndpoint>();

        // Call MapEndpoint on each
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app); 
        }

        return app;
    }

    /// <summary>
    /// Discovers and registers all IEndpoint implementations from an assembly
    /// </summary>
    public static IServiceCollection AddEndpointsFromAssembly(
        this IServiceCollection services,
        Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            // Find all types implementing IEndpoint in this assembly
            var endpointTypes = assembly.GetTypes()
                .Where(t => typeof(IEndpoint).IsAssignableFrom(t)
                         && t is { IsClass: true, IsAbstract: false });

            // Register each as IEndpoint
            foreach (var type in endpointTypes)
            {
                services.AddSingleton(typeof(IEndpoint), type);
            }
        }

        return services;
    }
}
