using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Delivery.Api.Districts;

internal sealed class GetDistricts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapAddressGroup()
            .MapGet("/districts", async ([FromQuery] int provinceId, IDeliveryService deliveryService) =>
            {
                var result = await deliveryService.GetDistrictsByProvinceAsync(provinceId);
                return result.MatchOk();
            })
            .WithName("GetDistricts")
            .WithDescription("Get districts by province ID from GHN")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

