using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Payment.Application.UseCases.PaymentConfigs.Commands.UpdatePaymentConfig;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Payment.Api.PaymentConfigs.UpdatePaymentConfig;

internal sealed class UpdatePaymentConfig : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/payments/config", async (
                UpdatePaymentConfigRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdatePaymentConfigCommand(
                    request.OnlinePaymentExpiredInDays,
                    request.OnlineTransactionExpiredInMinutes);
                var result = await sender.Send(command, cancellationToken);
                return result.MatchOk();
            })
            .WithTags("Payment Configs")
            .WithName("UpdatePaymentConfig")
            .WithSummary("Update payment configuration")
            .RequireAuthorization(policy => policy.RequireRole(Roles.SystemAdministrator, Roles.BusinessManager))
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

public record UpdatePaymentConfigRequest(
    int? OnlinePaymentExpiredInDays,
    int? OnlineTransactionExpiredInMinutes);
