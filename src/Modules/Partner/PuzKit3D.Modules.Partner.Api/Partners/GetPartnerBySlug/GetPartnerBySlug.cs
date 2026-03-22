using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.Partners.Queries.GetPartnerBySlug;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Partner.Api.Partners.GetPartnerBySlug;

internal sealed class GetPartnerBySlug : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnersGroup()
            .MapGet("/slug/{slug}", async (
                string slug,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPartnerBySlugQuery(slug);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetPartnerBySlug")
            .WithSummary("Get partner by slug")
            .WithDescription("Retrieves a partner by slug. Anonymous users and customers see only active partners. Staff/Manager see all partners with full details.")
            .AllowAnonymous()
            .Produces<object>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
