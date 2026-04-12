using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.SharedKernel.Api.Endpoint;

namespace PuzKit3D.Modules.Catalog.Api;

public static class EndpointGroupExtension
{
    public static RouteGroupBuilder MapAssemblyMethodsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/assembly-methods")
            .WithTags("Assembly Methods");
    }

    public static RouteGroupBuilder MapTopicsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/topics")
            .WithTags("Topics");
    }

    public static RouteGroupBuilder MapMaterialsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/materials")
            .WithTags("Materials");
    }

    public static RouteGroupBuilder MapCapabilitiesGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/capabilities")
            .WithTags("Capabilities");
    }

    public static RouteGroupBuilder MapDrivesGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/drives")
            .WithTags("Drives");
    }

    public static RouteGroupBuilder MapFormulasGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/formulas")
            .WithTags("Formulas");
    }

    public static RouteGroupBuilder MapFormulaValueValidationsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/formula-value-validations")
            .WithTags("Formula Value Validations");
    }
    
    public static RouteGroupBuilder MapFilterGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/filters")
            .WithTags("Filter");
    }
}
