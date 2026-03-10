using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Commands.UpdateInstockProductPriceDetail;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockProductPriceDetails.UpdateInstockProductPriceDetail;

internal sealed class UpdateInstockProductPriceDetail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPriceDetailsGroup()
            .MapPut("/{priceDetailId:guid}", async (
                Guid priceDetailId,
                [FromBody] UpdateInstockProductPriceDetailRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateInstockProductPriceDetailCommand(
                    priceDetailId,
                    request.UnitPrice);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdateInstockProductPriceDetail")
            .WithSummary("Update a price detail (Staff/Manager only)")
            .WithDescription("Updates a price detail. Only provided fields will be updated, null fields will be ignored. Unit price must be at least 10,000. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateInstockProductPriceDetailRequestDto(
    decimal? UnitPrice);
