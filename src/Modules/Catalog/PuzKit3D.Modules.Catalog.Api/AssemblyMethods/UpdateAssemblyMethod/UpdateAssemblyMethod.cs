using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Commands.UpdateAssemblyMethod;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.AssemblyMethods.UpdateAssemblyMethod;

internal sealed class UpdateAssemblyMethod : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAssemblyMethodsGroup()
            .MapPut("/{id:guid}", async (
                Guid id,
                [FromBody] UpdateAssemblyMethodRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateAssemblyMethodCommand(
                    id,
                    request.Name,
                    request.Slug,
                    request.Description,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("UpdateAssemblyMethod")
            .WithSummary("Update an assembly method")
            .WithDescription("Updates an existing assembly method by ID")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateAssemblyMethodRequestDto(
string Name,
string Slug,
string? Description,
bool IsActive);
