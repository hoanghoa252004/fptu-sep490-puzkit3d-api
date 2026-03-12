using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Payment.Application.Abstractions;
using PuzKit3D.Modules.Payment.Application.UseCases.Payments;
using PuzKit3D.Modules.Payment.Domain.Entities.Payments;
using PuzKit3D.Modules.Payment.Domain.Entities.Transactions;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PuzKit3D.Modules.Payment.Api.Payments.CreatePaymentUrl;

internal sealed class CreatePaymentUrl : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPaymentsGroup()
            .MapPost("/url", async (
                [FromBody] Guid OrderId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                //var paymentCode = $"{request.OrderType}_{request.OrderId}_{DateTime.UtcNow:yyyyMMddHHmmss}";

                var command = new CreatePaymentUrlCommand();
                var result = await sender.Send(command, cancellationToken);
                return result.MatchOk();
            })
            .WithName("CreatePaymentUrl")
            .WithSummary("Create payment URL for an order")
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
