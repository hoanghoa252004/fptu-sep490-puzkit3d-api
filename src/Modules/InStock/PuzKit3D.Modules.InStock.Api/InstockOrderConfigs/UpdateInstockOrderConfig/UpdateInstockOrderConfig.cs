using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockOrderConfigs.Commands.UpdateInstockOrderConfig;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockOrderConfigs.UpdateInstockOrderConfig;

internal sealed class UpdateInstockOrderConfig : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapOrdersGroup()
            .MapPut("api/configs/order", async (
                UpdateInstockOrderConfigRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateInstockOrderConfigCommand(
                    request.OrderMustCompleteInDays);
                var result = await sender.Send(command, cancellationToken);
                return result.MatchOk();
            })
            .WithTags("Instock Order Configs")
            .WithName("UpdateInstockOrderConfig")
            .WithSummary("Update instock order configuration")
            .RequireAuthorization(policy => policy.RequireRole(Roles.SystemAdministrator, Roles.BusinessManager))
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

public record UpdateInstockOrderConfigRequest(
    int? OrderMustCompleteInDays);
