using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Payment.Application.Abstractions;
using PuzKit3D.SharedKernel.Api.Endpoint;

namespace PuzKit3D.Modules.Payment.Api.Payments.HandleCallback;

internal sealed class HandleCallback : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        //app.MapPaymentsGroup()
        //    .MapGet("/callback", async (
        //        HttpContext httpContext,
        //        IPaymentGateway paymentGateway,
        //        CancellationToken cancellationToken) =>
        //    {
        //        var queryParams = httpContext.Request.Query
        //            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());

        //        var result = paymentGateway.ProcessCallback(queryParams);

        //        return result.Match(
        //            onSuccess: callbackResult => Results.Ok(new
        //            {
        //                paymentCode = callbackResult.PaymentCode,
        //                isSuccess = callbackResult.IsSuccess,
        //                transactionNo = callbackResult.TransactionNo,
        //                amount = callbackResult.Amount,
        //                message = callbackResult.Message
        //            }),
        //            onFailure: error => Results.BadRequest(new { error = error.Message })
        //        );
        //    })
        //    .WithName("HandlePaymentCallback")
        //    .WithSummary("Handle payment callback from VNPay")
        //    .WithDescription("Callback endpoint that receives payment result from VNPay. Returns payment status to frontend.")
        //    .Produces(StatusCodes.Status200OK)
        //    .ProducesProblem(StatusCodes.Status400BadRequest)
        //    .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
