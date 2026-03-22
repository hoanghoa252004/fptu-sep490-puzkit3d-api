using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Commands.PatchPartnerProduct;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Partner.Api.PartnerProducts.PatchPartnerProduct;

internal sealed class PatchPartnerProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductsGroup()
            .MapPatch("/{id:guid}/activate", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new PatchPartnerProductCommand(id);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("ActivatePartnerProduct")
            .WithSummary("Activate an partner product (Staff/Manager only)")
            .WithDescription("Activates an partner product by setting IsActive to true. Returns 400 if product is already active. Requires Staff or Manager role.")
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
