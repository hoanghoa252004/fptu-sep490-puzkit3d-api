using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PuzKit3D.Presentation.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Presentation.DependencyInjection.Extensions;

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
}