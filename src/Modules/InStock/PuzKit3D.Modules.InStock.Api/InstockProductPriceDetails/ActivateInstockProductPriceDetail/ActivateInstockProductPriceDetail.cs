using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Commands.ActivateInstockProductPriceDetail;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockProductPriceDetails.ActivateInstockProductPriceDetail;

internal sealed class ActivateInstockProductPriceDetail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPriceDetailsGroup()
            .MapPatch("/{priceDetailId:guid}/activate", async (
                Guid priceDetailId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new ActivateInstockProductPriceDetailCommand(priceDetailId);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("ActivateInstockProductPriceDetail")
            .WithSummary("Activate a price detail (Staff/Manager only)")
            .WithDescription("Activates a price detail by setting IsActive to true. Returns 400 if price detail is already active. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
