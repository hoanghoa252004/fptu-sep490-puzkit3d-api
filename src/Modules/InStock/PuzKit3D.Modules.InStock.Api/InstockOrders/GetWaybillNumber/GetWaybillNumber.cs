//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Routing;
//using PuzKit3D.Modules.Delivery.Application.Services;
//using PuzKit3D.Modules.InStock.Application.Repositories;
//using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
//using PuzKit3D.SharedKernel.Api.Endpoint;

//namespace PuzKit3D.Modules.InStock.Api.InstockOrders.GetWaybillNumber;

//internal sealed class GetWaybillNumber : IEndpoint
//{
//    public void MapEndpoint(IEndpointRouteBuilder app)
//    {
//        app.MapDeliveryInstockOrderGroup()
//            .MapGet("/waybill-number", async (
//                Guid orderId,
//                IInstockOrderRepository orderRepository,
//                IDeliveryService deliveryService,
//                CancellationToken cancellationToken) =>
//            {
//                var orderIdObj = InstockOrderId.From(orderId);
//                var order = await orderRepository.GetByIdAsync(orderIdObj, cancellationToken);

//                if (order == null)
//                {
//                    return Results.NotFound(new { error = "Order not found" });
//                }

//                if (string.IsNullOrWhiteSpace(order.DeliveryOrderCode))
//                {
//                    return Results.BadRequest(new { error = "Delivery order code not found for this order" });
//                }

//                // Generate token for the delivery order code
//                var tokenResult = await deliveryService.GeneratePrintTokenAsync(new List<string> { order.DeliveryOrderCode });

//                if (tokenResult.IsFailure)
//                {
//                    return Results.BadRequest(new { error = $"Failed to generate print token: {tokenResult.Error.Message}" });
//                }

//                // Get the print order URL using the token
//                var urlResult = await deliveryService.GetPrintOrderUrlAsync(tokenResult.Value);

//                if (urlResult.IsFailure)
//                {
//                    return Results.BadRequest(new { error = $"Failed to get print order URL: {urlResult.Error.Message}" });
//                }

//                return Results.Ok(new { waybillUrl = urlResult.Value });
//            })
//            .WithName("GetWaybillNumber")
//            .WithDescription("Get waybill number URL for an InStock Order")
//            .Produces<object>(StatusCodes.Status200OK)
//            .ProducesProblem(StatusCodes.Status400BadRequest)
//            .ProducesProblem(StatusCodes.Status404NotFound)
//            .ProducesProblem(StatusCodes.Status500InternalServerError);
//    }
//}
