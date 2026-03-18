using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Api;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Delivery.Api.Orders;

internal sealed class GetShippingOrderDetail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        //app.MapShippingGroup()
        //    .MapPost("/orders/detail", async (GetShippingOrderDetailRequest request, IDeliveryService deliveryService) =>
        //    {
        //        var result = await deliveryService.GetShippingOrderDetailAsync(request.OrderCode);
        //        return result.MatchOk();
        //    })
        //    .WithName("GetShippingOrderDetail")
        //    .WithDescription("Get shipping order detail from GHN")
        //    .AllowAnonymous()
        //    .Produces(StatusCodes.Status200OK)
        //    .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
