using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAssemblyMethodBySlug;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Catalog.Api.AssemblyMethods.GetAssemblyMethodBySlug;

internal sealed class GetAssemblyMethodBySlug : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAssemblyMethodsGroup()
            .MapGet("/slug/{slug}", async (
                string slug,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAssemblyMethodBySlugQuery(slug);

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetAssemblyMethodBySlug")
            .WithSummary("Get assembly method by slug")
            .WithDescription("Retrieves a single assembly method by its slug identifier")
            .AllowAnonymous()
            .Produces<GetAssemblyMethodBySlugResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
