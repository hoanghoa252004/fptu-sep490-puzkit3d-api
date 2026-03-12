using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Commands.CreateInstockOrder;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using System.Security.Claims;

namespace PuzKit3D.Modules.InStock.Api.InstockOrders.CreateInstockOrder;

internal sealed class CreateInstockOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapOrdersGroup()
            .MapPost("/", async (
                [FromBody] CreateInstockOrderRequestDto request,
                ClaimsPrincipal user,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var customerId))
                {
                    return Results.Unauthorized();
                }

                var cartItems = request.CartItems.Select(ci => new CartItemDto(
                    ci.ItemId,
                    ci.PriceDetailId,
                    ci.Quantity)).ToList();

                var command = new CreateInstockOrderCommand(
                    customerId,
                    request.CustomerName,
                    request.CustomerPhone,
                    request.CustomerEmail,
                    request.CustomerProvinceCode,
                    request.CustomerProvinceName,
                    request.CustomerDistrictCode,
                    request.CustomerDistrictName,
                    request.CustomerWardCode,
                    request.CustomerWardName,
                    cartItems,
                    request.SubTotalAmount,
                    request.ShippingFee,
                    request.UsedCoinAmount,
                    request.GrandTotalAmount,
                    request.PaymentMethod);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk(id => Results.Created($"/api/instock-orders/{id}", id));
            })
            .WithName("CreateInstockOrder")
            .WithSummary("Create a new instock order")
            .WithDescription("Creates a new instock order from cart items. Validates prices, calculates totals, and creates order details. Raises events for inventory update and cart clearing.")
            .RequireAuthorization()
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }
}

public sealed record CreateInstockOrderRequestDto(
    string CustomerName,
    string CustomerPhone,
    string CustomerEmail,
    string CustomerProvinceCode,
    string CustomerProvinceName,
    string CustomerDistrictCode,
    string CustomerDistrictName,
    string CustomerWardCode,
    string CustomerWardName,
    List<CartItemRequestDto> CartItems,
    decimal SubTotalAmount,
    decimal ShippingFee,
    int UsedCoinAmount,
    decimal GrandTotalAmount,
    string PaymentMethod);

public sealed record CartItemRequestDto(
    Guid ItemId,
    Guid PriceDetailId,
    int Quantity);
