using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Api;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Delivery.Api.Orders;

internal sealed class PrintOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        //app.MapShippingGroup()
        //    .MapPost("/orders/print", async (PrintOrderRequest request, IDeliveryService deliveryService) =>
        //    {
        //        // Generate token
        //        var tokenResult = await deliveryService.GeneratePrintTokenAsync(request.OrderCodes);
        //        if (!tokenResult.IsSuccess)
        //            return Results.BadRequest(tokenResult.Error);

        //        // Get print URL (A5 format only)
        //        var urlResult = await deliveryService.GetPrintOrderUrlAsync(tokenResult.Value);
        //        if (!urlResult.IsSuccess)
        //            return Results.BadRequest(urlResult.Error);

        //        // Return URL
        //        return Results.Ok(new 
        //        { 
        //            downloadUrl = urlResult.Value,
        //            format = "A5",
        //            orderCodes = request.OrderCodes
        //        });
        //    })
        //    .WithName("PrintOrder")
        //    .WithDescription("Get print shipping order label URL (A5 format)")
        //    .AllowAnonymous()
        //    .Produces(StatusCodes.Status200OK)
        //    .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
