using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Commands.UpdateInstockProductVariant;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockProductVariants.UpdateInstockProductVariant;

internal sealed class UpdateInstockProductVariant : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapVariantsGroup()
            .MapPut("/{variantId:guid}", async (
                Guid productId,
                Guid variantId,
                [FromBody] UpdateInstockProductVariantRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var previewImagesString = request.PreviewImages != null && request.PreviewImages.Count > 0
                    ? string.Join(",", request.PreviewImages)
                    : null;

                var command = new UpdateInstockProductVariantCommand(
                    variantId,
                    request.Color,
                    request.AssembledLengthMm,
                    request.AssembledWidthMm,
                    request.AssembledHeightMm,
                    previewImagesString,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdateInstockProductVariant")
            .WithSummary("Update an instock product variant (Staff/Manager only)")
            .WithDescription("Updates an instock product variant. Only provided fields will be updated, null fields will be ignored. SKU cannot be updated. Requires Staff or Manager role.")
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

internal sealed record UpdateInstockProductVariantRequestDto(
string? Color,
int? AssembledLengthMm,
int? AssembledWidthMm,
int? AssembledHeightMm,
List<string>? PreviewImages = null,
bool? IsActive = null);



