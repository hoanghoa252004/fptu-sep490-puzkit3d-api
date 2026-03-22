using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.SharedKernel.Api.Endpoint;

namespace PuzKit3D.Modules.Feedback.Api;

public static class EndpointGroupExtension
{
    public static RouteGroupBuilder MapFeedbacksGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup(ApiRoutes.ApiPrefix)
            .WithTags("Feedback");
    }
}
