using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace PuzKit3D.Modules.Delivery.Api;

public static class EndpointGroupExtension
{
    public static RouteGroupBuilder MapDeliveryGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup("api/delivery-trackings")
            .WithTags("Delivery Tracking");
    }
}
