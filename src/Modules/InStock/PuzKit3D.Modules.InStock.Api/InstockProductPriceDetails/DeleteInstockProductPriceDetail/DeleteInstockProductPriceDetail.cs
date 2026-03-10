using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Commands.DeleteInstockProductPriceDetail;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockProductPriceDetails.DeleteInstockProductPriceDetail;

internal sealed class DeleteInstockProductPriceDetail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPriceDetailsGroup()
            .MapDelete("/{priceDetailId:guid}", async (
                Guid priceDetailId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteInstockProductPriceDetailCommand(priceDetailId);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("DeleteInstockProductPriceDetail")
            .WithSummary("Deactivate a price detail (Staff/Manager only)")
            .WithDescription("Soft deletes a price detail by setting IsActive to false. Returns 400 if price detail is already inactive. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
