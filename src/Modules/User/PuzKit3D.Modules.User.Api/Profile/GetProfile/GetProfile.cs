using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Profile.Queries.GetProfile;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.User;

namespace PuzKit3D.Modules.User.Api.Profile.GetProfile;

internal sealed class GetProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapProfileGroup()
            .MapGet("/", async (
                ISender sender,
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {
                var query = new GetProfileQuery(currentUser.UserId!);
                
                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetProfile")
            .WithSummary("Get current user profile")
            .WithDescription("Retrieves the profile information of the currently authenticated user")
            .RequireAuthorization()
            .Produces<object>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
