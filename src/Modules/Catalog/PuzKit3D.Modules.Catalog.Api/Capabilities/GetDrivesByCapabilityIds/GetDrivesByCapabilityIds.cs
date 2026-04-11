using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.GetDrivesByCapabilityIds;
using PuzKit3D.Modules.Catalog.Application.UseCases.CapabilityDrives.Queries.Shared;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Catalog.Api.Capabilities.GetDrivesByCapabilityIds;

internal sealed class GetDrivesByCapabilityIds : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/drives", async (
                [FromBody] GetDrivesByCapabilityIdsRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetDrivesByCapabilityIdsQuery(request.CapabilityIds);
                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetDrivesByCapabilityIds")
            .WithSummary("Get active drives for a list of capabilities")
            .WithDescription("Retrieves a list of active drives associated with the provided capability IDs. Results are distinct and de-duplicated. Returns id and name for filtering purposes.")
            .Produces<List<GetDrivesByCapabilityIdsResponseDtos>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record GetDrivesByCapabilityIdsRequestDto(
    List<Guid> CapabilityIds);
