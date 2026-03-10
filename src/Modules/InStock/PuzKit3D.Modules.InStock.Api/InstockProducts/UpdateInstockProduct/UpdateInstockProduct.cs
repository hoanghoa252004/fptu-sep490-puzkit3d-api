using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Commands.UpdateInstockProduct;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockProducts.UpdateInstockProduct;

internal sealed class UpdateInstockProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapProductsGroup()
            .MapPut("/{id:guid}", async (
                Guid id,
                [FromBody] UpdateInstockProductRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateInstockProductCommand(
                    id,
                    request.Slug,
                    request.Name,
                    request.TotalPieceCount,
                    request.DifficultLevel,
                    request.EstimatedBuildTime,
                    request.ThumbnailUrl,
                    request.PreviewAsset,
                    request.TopicId,
                    request.AssemblyMethodId,
                    request.CapabilityId,
                    request.MaterialId,
                    request.Description);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdateInstockProduct")
            .WithSummary("Update an instock product (Staff/Manager only)")
            .WithDescription("Updates an instock product. Only provided fields will be updated, null fields will be ignored. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateInstockProductRequestDto(
    string? Slug,
    string? Name,
    int? TotalPieceCount,
    string? DifficultLevel,
    int? EstimatedBuildTime,
    string? ThumbnailUrl,
    Dictionary<string, string>? PreviewAsset,
    Guid? TopicId,
    Guid? AssemblyMethodId,
    Guid? CapabilityId,
    Guid? MaterialId,
    string? Description);
