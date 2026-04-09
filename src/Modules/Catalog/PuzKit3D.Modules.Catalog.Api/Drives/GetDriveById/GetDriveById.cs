using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Drives.Queries.GetDriveById;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Catalog.Api.Drives.GetDriveById;

internal sealed class GetDriveById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDrivesGroup()
            .MapGet("/{id:guid}", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetDriveByIdQuery(id);

                var result = await sender.Send(query, cancellationToken);

                return result.Match(
                    ok => Results.Ok(ok),
                    err => Results.BadRequest(err));
            })
            .WithName("GetDriveById")
            .WithSummary("Get a drive by ID")
            .WithDescription("Retrieves a drive by its ID.")
            .Produces<GetDriveByIdResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
