using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Api;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Delivery.Api.ShippingFee;

internal sealed class CalculateShippingFee : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapShippingGroup()
            .MapPost("/shipping-fee", async (CalculateShippingFeeRequest request, IDeliveryService deliveryService) =>
            {
                var result = await deliveryService.CalculateShippingFeeAsync(request);
                return result.MatchOk();
            })
            .WithName("CalculateShippingFee")
            .WithDescription("Calculate shipping fee from GHN")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
