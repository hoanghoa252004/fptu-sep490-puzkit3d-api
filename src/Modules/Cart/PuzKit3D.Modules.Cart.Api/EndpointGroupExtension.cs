using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.SharedKernel.Api.Endpoint;

namespace PuzKit3D.Modules.Cart.Api;

public static class EndpointGroupExtension
{
    public static RouteGroupBuilder MapInStockCartsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/instock-carts")
            .WithTags("InStock Carts");
    }

    public static RouteGroupBuilder MapPartnerCartsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/partner-carts")
            .WithTags("Partner Carts");
    }
}


