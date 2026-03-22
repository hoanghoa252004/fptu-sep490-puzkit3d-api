using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.Parts.Commands.CreatePart;
using PuzKit3D.Modules.InStock.Domain.Entities.Parts;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.Parts.CreatePart;

internal sealed class CreatePart : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartsGroup()
            .MapPost("/", async (
                Guid productId,
                [FromBody] CreatePartRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                // Parse PartType from string
                if (!Enum.TryParse<PartType>(request.PartType, ignoreCase: true, out var partType))
                {
                    return Results.BadRequest(new { error = $"Invalid part type '{request.PartType}'. Valid values are: {string.Join(", ", Enum.GetNames(typeof(PartType)))}" });
                }

                var command = new CreatePartCommand(
                    productId,
                    request.Name,
                    partType,
                    request.Quantity);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk(id => Results.Created($"/api/instock-products/{productId}/parts/{id}", id));
            })
            .WithName("CreatePart")
            .WithSummary("Create a new part for a product (Staff/Manager only)")
            .WithDescription("Creates a new part for the specified product. Code is auto-generated (PARxxxx). PartType must be one of: Structural, Mechanical, Decorative. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreatePartRequestDto(
string Name,
string PartType,
int Quantity);

