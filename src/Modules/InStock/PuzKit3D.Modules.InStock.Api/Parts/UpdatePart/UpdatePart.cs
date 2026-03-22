using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.Parts.Commands.UpdatePart;
using PuzKit3D.Modules.InStock.Domain.Entities.Parts;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.Parts.UpdatePart;

internal sealed class UpdatePart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartsGroup()
            .MapPut("/{partId:guid}", async (
                Guid productId,
                Guid partId,
                [FromBody] UpdatePartRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                // Parse PartType from string if provided
                PartType? partType = null;
                if (!string.IsNullOrWhiteSpace(request.PartType))
                {
                    if (!Enum.TryParse<PartType>(request.PartType, ignoreCase: true, out var parsedType))
                    {
                        return Results.BadRequest(new { error = $"Invalid part type '{request.PartType}'. Valid values are: {string.Join(", ", Enum.GetNames(typeof(PartType)))}" });
                    }
                    partType = parsedType;
                }

                var command = new UpdatePartCommand(
                    productId,
                    partId,
                    request.Name,
                    partType,
                    request.Quantity);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("UpdatePart")
            .WithSummary("Update a part (Staff/Manager only)")
            .WithDescription("Updates name and part type of an existing part. PartType must be one of: Structural, Mechanical, Decorative. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdatePartRequestDto(
string? Name,
string? PartType,
int? Quantity);
