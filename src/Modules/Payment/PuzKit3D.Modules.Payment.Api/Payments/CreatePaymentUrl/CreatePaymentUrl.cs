using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Payment.Application.UseCases.Payments;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using System.Net.Http;

namespace PuzKit3D.Modules.Payment.Api.Payments.CreatePaymentUrl;

internal sealed class CreatePaymentUrl : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPaymentsGroup()
            .MapPost("/url", async (
                [FromBody] CreatePaymentUrlRequest request,
                HttpContext httpContext,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreatePaymentUrlCommand(httpContext, request.OrderId, request.provider);
                var result = await sender.Send(command, cancellationToken);
                return result.MatchOk();
            })
            .WithName("CreatePaymentUrl")
            .WithSummary("Create payment URL for an order")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))

            .Produces<CreatePaymentUrlResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
