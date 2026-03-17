using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Delivery.Api.Provinces;

internal sealed class GetProvinces : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAddressGroup()
            .MapGet("/provinces", async (IDeliveryService deliveryService) =>
            {
                var result = await deliveryService.GetProvincesAsync();
                return result.MatchOk();
            })
            .WithName("GetProvinces")
            .WithDescription("Get all provinces from GHN")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
