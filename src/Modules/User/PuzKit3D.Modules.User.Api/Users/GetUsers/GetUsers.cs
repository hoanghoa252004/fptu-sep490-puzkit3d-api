using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.User.Application.UseCases.Users.Queries.GetUsers;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authentication.Dtos;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.User.Api.Users.GetUsers;

internal sealed class GetUsers : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapUsersGroup()
            .MapGet("/", async (
                int pageNumber,
                int pageSize,
                string? searchTerm,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetUsersQuery(pageNumber, pageSize, searchTerm);
                
                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetUsers")
            .WithSummary("[Admin]")
            .WithDescription("Retrieves a paginated list of users with optional search filtering. Requires Admin role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.SystemAdministrator))
            .Produces<GetUsersResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
