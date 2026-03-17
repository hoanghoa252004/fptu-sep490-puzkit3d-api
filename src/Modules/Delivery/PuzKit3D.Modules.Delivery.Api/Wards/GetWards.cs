using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Delivery.Api.Wards;

internal sealed class GetWards : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAddressGroup()
            .MapGet("/wards", async ([FromQuery ]int districtId, IDeliveryService deliveryService) =>
            {
                var result = await deliveryService.GetWardsByDistrictAsync(districtId);
                return result.MatchOk();
            })
            .WithName("GetWards")
            .WithDescription("Get wards by district ID from GHN")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

