using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Commands.CreateInstockProductVariant;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockProductVariants.CreateInstockProductVariant;

internal sealed class CreateInstockProductVariant : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapVariantsGroup()
            .MapPost("/", async (
                Guid productId,
                [FromBody] CreateInstockProductVariantRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateInstockProductVariantCommand(
                    productId,
                    request.Sku,
                    request.Color,
                    request.AssembledLengthMm,
                    request.AssembledWidthMm,
                    request.AssembledHeightMm,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk(id => Results.Created($"/api/instock-products/{productId}/variants/{id}", id));
            })
            .WithName("CreateInstockProductVariant")
            .WithSummary("Create a new instock product variant (Staff/Manager only)")
            .WithDescription("Creates a new variant for an instock product. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateInstockProductVariantRequestDto(
    string Sku,
    string Color,
    int AssembledLengthMm,
    int AssembledWidthMm,
    int AssembledHeightMm,
    bool IsActive);
