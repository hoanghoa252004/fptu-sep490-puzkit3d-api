using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Payment.Application.UseCases.Transactions.Queries.GetTransactionsByPayment;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Payment.Api.Transactions.GetTransactionsByPayment;

internal sealed class GetTransactionsByPayment : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPaymentsGroup()
            .MapGet("/{paymentId}/transactions", async (
                Guid paymentId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPaymentTransactionsQuery(paymentId);
                var result = await sender.Send(query, cancellationToken);
                return result.MatchOk();
            })
            .WithName("GetPaymentTransactions")
            .WithSummary("Get all transactions for a payment")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))
            .Produces<GetPaymentTransactionsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
