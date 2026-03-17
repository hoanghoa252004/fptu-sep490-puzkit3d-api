using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Api.Endpoint;
using System.Text.Json;

namespace PuzKit3D.Modules.InStock.Api.InstockOrders.GetDeliveryTracking;

internal sealed class GetDeliveryTracking : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDeliveryInstockOrderGroup()
            .MapGet("/", async (
                Guid orderId,
                IInstockOrderRepository orderRepository,
                IDeliveryService deliveryService,
                CancellationToken cancellationToken) =>
            {
                var orderIdObj = InstockOrderId.From(orderId);
                var order = await orderRepository.GetByIdAsync(orderIdObj, cancellationToken);

                if (order == null)
                {
                    return Results.NotFound(new { error = "Order not found" });
                }

                if (string.IsNullOrWhiteSpace(order.DeliveryOrderCode))
                {
                    return Results.BadRequest(new { error = "Delivery order code not found for this order" });
                }

                var result = await deliveryService.GetShippingOrderDetailAsync(order.DeliveryOrderCode);

                if (result.IsFailure)
                {
                    return Results.BadRequest(new { error = result.Error.Message });
                }

                try
                {
                    // Serialize the response to JSON and parse it
                    var json = JsonSerializer.Serialize(result.Value);
                    using var doc = JsonDocument.Parse(json);
                    
                    // Navigate to data.status
                    var dataProperty = doc.RootElement.GetProperty("data");
                    var status = dataProperty.GetProperty("status").GetString();

                    return Results.Ok(new { status = status });
                }
                catch (JsonException ex)
                {
                    return Results.BadRequest(new { error = $"Invalid response format from delivery service: {ex.Message}" });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = $"Failed to parse delivery service response: {ex.Message}" });
                }
            })
            .WithName("GetDeliveryTracking")
            .WithDescription("Get delivery tracking status for an InStock Order")
            .Produces<object>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
