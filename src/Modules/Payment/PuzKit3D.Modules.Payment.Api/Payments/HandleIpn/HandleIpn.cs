using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Payment.Application.Abstractions;
using PuzKit3D.SharedKernel.Api.Endpoint;

namespace PuzKit3D.Modules.Payment.Api.Payments.HandleIpn;

internal sealed class HandleIpn : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        //app.MapPaymentsGroup()
        //    .MapGet("/ipn", async (
        //        HttpContext httpContext,
        //        IPaymentGateway paymentGateway,
        //        CancellationToken cancellationToken) =>
        //    {
        //        var queryParams = httpContext.Request.Query
        //            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());

        //        var result = paymentGateway.ProcessCallback(queryParams);

        //        if (result.IsFailure)
        //        {
        //            return Results.BadRequest(new { RspCode = "97", Message = "Invalid Signature" });
        //        }

        //        var callbackResult = result.Value;

        //        // TODO: Process payment completion here
        //        // - Update Payment entity status
        //        // - Create Transaction record
        //        // - Update order status
        //        // - Send notifications, etc.

        //        if (callbackResult.IsSuccess)
        //        {
        //            return Results.Ok(new { RspCode = "00", Message = "Confirm Success" });
        //        }

        //        return Results.Ok(new { RspCode = "99", Message = "Unknown error" });
        //    })
        //    .WithName("HandlePaymentIpn")
        //    .WithSummary("Handle IPN (Instant Payment Notification) from VNPay")
        //    .WithDescription("IPN endpoint for VNPay to notify payment status. This is where you process the payment (update database, etc.). Must return proper response codes.")
        //    .Produces(StatusCodes.Status200OK)
        //    .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
