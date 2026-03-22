using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Commands.UpdatePartnerProduct;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Partner.Api.PartnerProducts.UpdatePartnerProduct;

internal sealed class UpdatePartnerProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductsGroup()
            .MapPut("/{id}", async (
                Guid id,
                [FromBody] UpdatePartnerProductRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdatePartnerProductCommand(
                    id,
                    request.Name,
                    request.ReferencePrice,
                    request.ThumbnailUrl,
                    request.PreviewAsset,
                    request.Slug,
                    request.Description);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("UpdatePartnerProduct")
            .WithSummary("Update a partner product (Staff/Manager only)")
            .WithDescription("Updates an existing partner product with new name, slug, reference price, and other details. IsActive cannot be updated via this endpoint. Requires Staff or Manager role.")
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

internal sealed record UpdatePartnerProductRequestDto(
    string Name,
    decimal ReferencePrice,
    string ThumbnailUrl,
    Dictionary<string, string> PreviewAsset,
    string Slug,
    string? Description = null);
