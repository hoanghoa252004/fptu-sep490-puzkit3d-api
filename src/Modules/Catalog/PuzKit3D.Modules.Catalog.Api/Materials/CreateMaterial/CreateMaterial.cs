using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Commands.CreateMaterial;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.Materials.CreateMaterial;

internal sealed class CreateMaterial : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapMaterialsGroup()
            .MapPost("/", async (
                [FromBody] CreateMaterialRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateMaterialCommand(
                    request.Name,
                    request.Slug,
                    request.Description,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetMaterialById", id => new { id });
            })
            .WithName("CreateMaterial")
            .WithSummary("Create a new material (Staff/Manager only)")
            .WithDescription("Creates a new material with name, slug, description, and active status. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateMaterialRequestDto(
    string Name,
    string Slug,
    string? Description,
    bool IsActive);
