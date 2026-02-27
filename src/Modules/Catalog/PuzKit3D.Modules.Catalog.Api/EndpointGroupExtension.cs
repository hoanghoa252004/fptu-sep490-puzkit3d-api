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
}
