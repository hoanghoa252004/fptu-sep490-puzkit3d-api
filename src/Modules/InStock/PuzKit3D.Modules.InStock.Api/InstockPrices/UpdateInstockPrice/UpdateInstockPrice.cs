using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Commands.UpdateInstockPrice;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockPrices.UpdateInstockPrice;

internal sealed class UpdateInstockPrice : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPricesGroup()
            .MapPut("/{priceId:guid}", async (
                Guid priceId,
                [FromBody] UpdateInstockPriceRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateInstockPriceCommand(
                    priceId,
                    request.Name,
                    request.EffectiveFrom,
                    request.EffectiveTo,
                    request.Priority,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdateInstockPrice")
            .WithSummary("Update an instock price (Staff/Manager only)")
            .WithDescription("Updates an instock price. Only provided fields will be updated, null fields will be ignored. Requires Staff or Manager role.")
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

internal sealed record UpdateInstockPriceRequestDto(
string? Name,
DateTime? EffectiveFrom,
DateTime? EffectiveTo,
int? Priority,
bool? IsActive);
