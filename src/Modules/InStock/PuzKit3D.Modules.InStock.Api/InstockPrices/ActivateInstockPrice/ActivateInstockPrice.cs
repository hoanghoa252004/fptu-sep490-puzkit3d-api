using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Commands.ActivateInstockPrice;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockPrices.ActivateInstockPrice;

internal sealed class ActivateInstockPrice : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPricesGroup()
            .MapPatch("/{priceId:guid}/activate", async (
                Guid priceId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new ActivateInstockPriceCommand(priceId);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("ActivateInstockPrice")
            .WithSummary("Activate an instock price (Staff/Manager only)")
            .WithDescription("Activates an instock price by setting IsActive to true. Returns 400 if price is already active. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
