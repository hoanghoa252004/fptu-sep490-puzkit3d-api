using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Commands.CreatePartnerProductOrder;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Partner.Api.PartnerProductOrders.CreatePartnerProductOrder;

internal sealed class CreatePartnerProductOrder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductOrdersGroup()
            .MapPost("/", async (
                [FromBody] CreatePartnerProductOrderRequestDto request,
                ISender sender,
                IHttpContextAccessor httpContextAccessor,
                CancellationToken cancellationToken) =>
            {
                var customerId = httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?
                    .Value;
                if (string.IsNullOrEmpty(customerId) || !Guid.TryParse(customerId, out var customerGuid))
                {
                    return Results.Unauthorized();
                }
                var items = request.Items?
                    .ConvertAll(i => new PartnerProductOrderDetailsItemDto(
                        i.PartnerProductId,
                        i.Quantity,
                        i.Price));

                var command = new CreatePartnerProductOrderCommand(
                    request.QuotationId,
                    customerGuid,
                    request.CustomerName,
                    request.CustomerPhone,
                    request.CustomerEmail,
                    request.CustomerProvinceName,
                    request.CustomerDistrictName,
                    request.CustomerWardName,
                    request.DetailAddress,
                    request.UserCoinAmount,
                    request.ShippingFee,
                    request.PaymentMethod,
                    items);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetPartnerProductOrderById", id => new { id });
            })
            .WithName("CreatePartnerProductOrder")
            .WithSummary("Create a new partner product order (Customer only)")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    public sealed record CreatePartnerProductOrderRequestDto(
        Guid QuotationId,
        string CustomerName,
        string CustomerPhone,
        string CustomerEmail,
        string CustomerProvinceName,
        string CustomerDistrictName,
        string CustomerWardName,
        string DetailAddress,
        int UserCoinAmount,
        decimal ShippingFee,
        string PaymentMethod,
        List<PartnerProductOrderDetailsItemRequestDto> Items);

    public sealed record PartnerProductOrderDetailsItemRequestDto(
        Guid PartnerProductId,
        int Quantity,
        decimal Price);
}
