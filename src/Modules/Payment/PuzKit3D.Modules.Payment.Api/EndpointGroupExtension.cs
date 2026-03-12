using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.SharedKernel.Api.Endpoint;

namespace PuzKit3D.Modules.Payment.Api;

public static class EndpointGroupExtension
{
    public static RouteGroupBuilder MapPaymentsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/payments")
            .WithTags("Payments");
    }
}
