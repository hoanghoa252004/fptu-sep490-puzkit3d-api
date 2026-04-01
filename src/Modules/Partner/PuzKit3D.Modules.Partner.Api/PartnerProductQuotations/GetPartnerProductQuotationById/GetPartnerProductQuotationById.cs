using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Queries.GetPartnerProductQuotationById;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Partner.Api.PartnerProductQuotations.GetPartnerProductQuotationById;

internal sealed class GetPartnerProductQuotationById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductQuotationsGroup()
            .MapGet("/{id}", async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPartnerProductQuotationByIdQuery(id);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetPartnerProductQuotationById")
            .WithSummary("Get partner product quotation by id with details")
            .Produces<GetPartnerProductQuotationByIdResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
