using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Queries.GetPartnerProductQuotationByRequestId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Partner.Api.PartnerProductQuotations.GetPartnerProductQuotationByRequestId;

internal sealed class GetPartnerProductQuotationByRequestId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductQuotationsGroup()
            .MapGet("/by-request/{requestId}", async (
                Guid requestId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPartnerProductQuotationByRequestIdQuery(requestId);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetPartnerProductQuotationByRequestId")
            .WithSummary("Get partner product quotation by request id with details")
            .Produces<GetPartnerProductQuotationByRequestIdResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
