using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Queries.GetAllDrives;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Catalog.Api.Drives.GetAllDrives;

internal sealed class GetAllDrives : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDrivesGroup()
            .MapGet("/", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllDrivesQuery();

                var result = await sender.Send(query, cancellationToken);

                return result.Match(
                    ok => Results.Ok(ok),
                    err => Results.BadRequest(err));
            })
            .WithName("GetAllDrives")
            .WithSummary("Get all drives")
            .WithDescription("Retrieves all drives.")
            .Produces<IEnumerable<GetAllDrivesResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
