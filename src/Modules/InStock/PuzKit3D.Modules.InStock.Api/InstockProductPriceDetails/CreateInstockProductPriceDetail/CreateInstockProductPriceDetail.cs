using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Commands.CreateInstockProductPriceDetail;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockProductPriceDetails.CreateInstockProductPriceDetail;

internal sealed class CreateInstockProductPriceDetail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPriceDetailsGroup()
            .MapPost("/", async (
                [FromBody] CreateInstockProductPriceDetailRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateInstockProductPriceDetailCommand(
                    request.PriceId,
                    request.VariantId,
                    request.UnitPrice,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk(id => Results.Created($"/api/instock-price-details/{id}", id));
            })
            .WithName("CreateInstockProductPriceDetail")
            .WithSummary("Create a new price detail (Staff/Manager only)")
            .WithDescription("Assigns a price to a product variant with a unit price. Unit price must be at least 10,000. Requires Staff or Manager role.")
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

internal sealed record CreateInstockProductPriceDetailRequestDto(
    Guid PriceId,
    Guid VariantId,
    decimal UnitPrice,
    bool IsActive);
