using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Users.Queries.GetUserById;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.User.Api.Users.GetUserById;

internal sealed class GetUserById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapUsersGroup()
            .MapGet("/{id}", async (
                string id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetUserByIdQuery(id);
                
                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetUserById")
            .WithSummary("Get user by ID (Admin/Manager only)")
            .WithDescription("Retrieves detailed information about a specific user")
            .RequireAuthorization(Permissions.Users.ViewUsers)
            .Produces<object>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
