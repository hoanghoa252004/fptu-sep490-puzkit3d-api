using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Mappers;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Api;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Delivery.Api.Orders;

internal sealed class CreateShippingOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapShippingGroup()
            .MapPost("/orders", async (CreateShippingOrderRequest request, IDeliveryService deliveryService) =>
            {
                var ghnRequest = request.ToGhnRequest();
                var result = await deliveryService.CreateShippingOrderAsync(ghnRequest);
                return result.MatchOk();
            })
            .WithName("CreateShippingOrder")
            .WithDescription("Create a shipping order with GHN")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
