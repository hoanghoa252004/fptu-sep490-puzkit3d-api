using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Queries.GetInstockProductPriceDetailById;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockProductPriceDetails.GetInstockProductPriceDetailById;

internal sealed class GetInstockProductPriceDetailById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPriceDetailsGroup()
            .MapGet("/{priceDetailId:guid}", async (
                Guid priceDetailId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetInstockProductPriceDetailByIdQuery(priceDetailId);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetInstockProductPriceDetailById")
            .WithSummary("Get price detail by ID (Staff/Manager only)")
            .WithDescription("Retrieves a specific price detail by its ID. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<GetInstockProductPriceDetailByIdResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
