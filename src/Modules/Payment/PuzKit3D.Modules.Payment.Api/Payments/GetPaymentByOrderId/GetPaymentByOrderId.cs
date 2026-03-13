using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Payment.Application.UseCases.Payments.Queries.GetPaymentByOrderId;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Payment.Api.Payments.GetPaymentByOrderId;

internal sealed class GetPaymentByOrderId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/orders/{orderId}/payments", async (
                Guid orderId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPaymentByOrderIdQuery(orderId);
                var result = await sender.Send(query, cancellationToken);
                return result.MatchOk();
            })
            .WithTags("Payments")
            .WithName("GetPaymentByOrderId")
            .WithSummary("Get payment information for an order")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))
            .Produces<GetPaymentByOrderIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
