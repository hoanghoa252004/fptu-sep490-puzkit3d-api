using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.SharedKernel.Api.Endpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.CustomDesign.Api;

public static class EndpointGroupExtension
{
    public static RouteGroupBuilder MapCustomDesignRequirementGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/custom-design-requirements")
            .WithTags("Custom Design Requirements");
    }

    public static RouteGroupBuilder MapCustomDesignRequestGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/custom-design-requests")
            .WithTags("Custom Design Requests");
    }

    public static RouteGroupBuilder MapCustomDesignAssetGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/custom-design-assets")
            .WithTags("Custom Design Assets");
    }
}