using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Payment.Application.UseCases.Transactions.Commands.CreateTransaction;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;
using System.Net.Http;

namespace PuzKit3D.Modules.Payment.Api.Transactions.CreateTransaction;

internal sealed class CreateTransaction : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPaymentsGroup()
            .MapPost("/{paymentId}/transactions", async (
                [FromBody] CreateTransactionRequestDto request,
                HttpContext httpContext,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateTransactionCommand(httpContext, request.paymentId, request.provider);
                var result = await sender.Send(command, cancellationToken);
                return result.MatchOk();
            })
            .WithName("CreateTransaction")
            .WithSummary("Create payment URL for an order")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))

            .Produces<string>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
