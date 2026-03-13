using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Payment.Application.UseCases.Transactions.Commands.ProcessVnPayIPN;
using PuzKit3D.SharedKernel.Api.Endpoint;
using System.Text.Json.Serialization;

namespace PuzKit3D.Modules.Payment.Api.Payments.IPN;

internal sealed class IPN : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/ipn", async (
            HttpContext httpContext,
            ISender sender,
            CancellationToken cancellationToken) =>
            {
                try
                {
                    var command = new ProcessVnPayIPNCommand(httpContext.Request.Query);
                    var result = await sender.Send(command, cancellationToken);

                    if (result.IsSuccess)
                    {
                        return Results.Json(
                            new VnPayIPNResponseDto { RspCode = "00", Message = "Confirm Success" },
                            statusCode: StatusCodes.Status200OK);
                    }

                    // Handle different error codes from the domain/application layer
                    var errorCode = result.Error?.Code ?? "99";
                    var errorMessage = result.Error?.Message ?? "Internal error";

                    return Results.Json(
                        new VnPayIPNResponseDto { RspCode = errorCode, Message = errorMessage },
                        statusCode: GetStatusCode(errorCode));
                }
                catch (Exception)
                {
                    return Results.Json(
                        new VnPayIPNResponseDto { RspCode = "99", Message = "Internal error" },
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithTags("Payments")
            .WithName("IPN Callback")
            .WithSummary("Handle IPN (Instant Payment Notification)")
            .Produces<VnPayIPNResponseDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    private static int GetStatusCode(string rspCode)
    {
        return rspCode switch
        {
            "00" => StatusCodes.Status200OK,
            "97" => StatusCodes.Status400BadRequest,
            "99" => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }
}
