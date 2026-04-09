using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.GetAllCapabilityDrives;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Catalog.Api.CapabilityDrives.GetAllCapabilityDrives;

internal sealed class GetAllCapabilityDrives : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCapabilityDrivesGroup()
            .MapGet("/", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllCapabilityDrivesQuery();

                var result = await sender.Send(query, cancellationToken);

                return result.Match(
                    ok => Results.Ok(ok),
                    err => Results.BadRequest(err));
            })
            .WithName("GetAllCapabilityDrives")
            .WithSummary("Get all capability-drive links")
            .WithDescription("Retrieves all capability-drive links.")
            .Produces<IEnumerable<GetAllCapabilityDrivesResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
