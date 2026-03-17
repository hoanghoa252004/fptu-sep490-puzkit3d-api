using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Commands.CreateDeliverTrackingOrder;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Api.InstockOrders.GetGhnOrderCode;

internal sealed class CreateDeliverTrackingOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDeliveryInstockOrderGroup()
            .MapPost("/", async (
                Guid orderId,
                ISender sender,
                IInstockOrderRepository orderRepository,
                IInStockUnitOfWork unitOfWork,
                CancellationToken cancellationToken) =>
            {
                var query = new CreateDeliverTrackingOrderCommand(orderId);
                var result = await sender.Send(query, cancellationToken);
                
                if (result.IsFailure)
                {
                    return Results.BadRequest(new { error = result.Error.Message });
                }

                // Get the GHN response
                var ghnResponse = result.Value;

                // Update order with delivery information
                var orderIdObj = InstockOrderId.From(orderId);
                var order = await orderRepository.GetByIdAsync(orderIdObj, cancellationToken);

                if (order == null)
                {
                    return Results.NotFound(new { error = "Order not found" });
                }

                // Set delivery info and get expected delivery date from GHN
                var setDeliveryResult = order.SetDeliveryInfo(
                    ghnResponse.DeliveryOrderCode,
                    ghnResponse.ExpectedDeliveryTime);

                if (setDeliveryResult.IsFailure)
                {
                    return Results.BadRequest(new { error = setDeliveryResult.Error.Message });
                }

                // Save to database
                await unitOfWork.ExecuteAsync(async () =>
                {
                    orderRepository.Update(order);
                    return Result.Success();
                }, cancellationToken);

                return Results.Ok(new { deliveryOrderCode = ghnResponse.DeliveryOrderCode });
            })
            .WithName("GetGhnOrderCode")
            .WithDescription("Create GHN shipping order and get delivery order code by InstockOrder ID")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
