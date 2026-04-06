using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Payment.Application.UseCases.PaymentConfigs.Queries.GetPaymentConfig;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Payment.Api.PaymentConfigs.GetPaymentConfig;

internal sealed class GetPaymentConfig : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/configs/payment", async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPaymentConfigQuery();
                var result = await sender.Send(query, cancellationToken);
                return result.MatchOk();
            })
            .WithTags("Payment Configs")
            .WithName("GetPaymentConfig")
            .WithSummary("Get payment configuration")
            .AllowAnonymous()
            .Produces<GetPaymentConfigResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}
