using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Api;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Delivery.Api.ShippingFee;

internal sealed class CalculateShippingFeeByLocation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapShippingGroup()
            .MapPost("/calculate-fee-by-location", async (CalculateShippingFeeByLocationRequest request, IDeliveryService deliveryService) =>
            {
                var result = await deliveryService.CalculateShippingFeeByLocationAsync(request);
                if (!result.IsSuccess)
                    return Results.BadRequest(result.Error);
                
                return Results.Ok(result.Value);
            })
            .WithName("CalculateShippingFeeByLocation")
            .WithDescription("Calculate shipping fee by province, district, and ward names (weight = 1)")
            .AllowAnonymous()
            .Produces<int>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
