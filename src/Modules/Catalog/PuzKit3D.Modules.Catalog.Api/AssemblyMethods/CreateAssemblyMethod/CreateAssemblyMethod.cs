using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Commands.CreateAssemblyMethod;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Catalog.Api.AssemblyMethods.CreateAssemblyMethod;

internal sealed class CreateAssemblyMethod : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAssemblyMethodsGroup()
            .MapPost("/", async (
                [FromBody] CreateAssemblyMethodRequestDto request, 
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var command = new CreateAssemblyMethodCommand(
                    request.Name,
                    request.Slug,
                    request.Description,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetAssemblyMethodById", id => new { id });
            })
            .WithName("CreateAssemblyMethod")
            .WithSummary("Create a new assembly method")
            .WithDescription("Creates a new assembly method with name, slug, description, and active status")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateAssemblyMethodRequestDto(
    string Name,
    string Slug,
    string? Description,
    bool IsActive);
