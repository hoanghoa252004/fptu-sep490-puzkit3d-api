using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.SharedKernel.Api.Endpoint;

namespace PuzKit3D.Modules.Partner.Api;

public static class EndpointGroupExtension
{
    public static RouteGroupBuilder MapPartnerProductsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/partner-products")
            .WithTags("Partner Products");
    }

    public static RouteGroupBuilder MapPartnersGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/partners")
            .WithTags("Partners");
    }
}
