using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Commands.DeleteInstockPrice;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockPrices.DeleteInstockPrice;

internal sealed class DeleteInstockPrice : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPricesGroup()
            .MapDelete("/{priceId:guid}", async (
                Guid priceId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteInstockPriceCommand(priceId);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("DeleteInstockPrice")
            .WithSummary("Deactivate an instock price (Staff/Manager only)")
            .WithDescription("Soft deletes an instock price by setting IsActive to false. Returns 400 if price is already inactive. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
