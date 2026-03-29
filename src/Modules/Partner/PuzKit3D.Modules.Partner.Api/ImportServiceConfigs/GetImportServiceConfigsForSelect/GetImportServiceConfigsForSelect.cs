using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Queries.GetImportServiceConfigsForSelect;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Partner.Api.ImportServiceConfigs.GetImportServiceConfigsForSelect;

internal sealed class GetImportServiceConfigsForSelect : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapImportServiceConfigsGroup()
            .MapGet("/select", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetImportServiceConfigsForSelectQuery();

                var result = await sender.Send(query, cancellationToken);

                return result.MatchOk();
            })
            .WithName("GetImportServiceConfigsForSelect")
            .WithSummary("Get all active import service configs for select/dropdown")
            .WithDescription("Retrieves all active import service configs with only Id, CountryName, and CountryCode fields. Suitable for select/dropdown components.")
            .AllowAnonymous()
            .Produces<IEnumerable<object>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
