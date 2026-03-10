using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Commands.ActivateInstockProductVariant;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockProductVariants.ActivateInstockProductVariant;

internal sealed class ActivateInstockProductVariant : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapVariantsGroup()
            .MapPatch("/{variantId:guid}/activate", async (
                Guid productId,
                Guid variantId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new ActivateInstockProductVariantCommand(variantId);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("ActivateInstockProductVariant")
            .WithSummary("Activate an instock product variant (Staff/Manager only)")
            .WithDescription("Activates an instock product variant by setting IsActive to true. Returns 400 if variant is already active. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
