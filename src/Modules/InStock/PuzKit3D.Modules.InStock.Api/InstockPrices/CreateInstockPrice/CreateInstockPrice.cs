using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Commands.CreateInstockPrice;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockPrices.CreateInstockPrice;

internal sealed class CreateInstockPrice : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPricesGroup()
            .MapPost("/", async (
                [FromBody] CreateInstockPriceRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateInstockPriceCommand(
                    request.Name,
                    request.EffectiveFrom,
                    request.EffectiveTo,
                    request.Priority,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk(id => Results.Created($"/api/instock-prices/{id}", id));
            })
            .WithName("CreateInstockPrice")
            .WithSummary("Create a new instock price (Staff/Manager only)")
            .WithDescription("Creates a new instock price with effective date range and priority. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateInstockPriceRequestDto(
    string Name,
    DateTime EffectiveFrom,
    DateTime EffectiveTo,
    int Priority,
    bool IsActive);
