using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace PuzKit3D.Modules.User.Api;

public static class EndpointGroupExtension
{
    public static RouteGroupBuilder MapAuthenticationGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup("api/auth")
            .WithTags("Authentication");
    }

    public static RouteGroupBuilder MapUsersGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup("api/users")
            .WithTags("Users");
    }
}
