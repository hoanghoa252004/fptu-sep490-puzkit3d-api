using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Commands.UpdateImportServiceConfig;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Partner.Api.ImportServiceConfigs.UpdateImportServiceConfig;

internal sealed class UpdateImportServiceConfig : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapImportServiceConfigsGroup()
            .MapPut("/{id:guid}", async (
                Guid id,
                [FromBody] UpdateImportServiceConfigRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateImportServiceConfigCommand(
                    id,
                    request.BaseShippingFee,
                    request.CountryCode,
                    request.CountryName,
                    request.ImportTaxPercentage,
                    request.EstimatedDeliveryDays);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("UpdateImportServiceConfig")
            .WithSummary("Update an import service config (Manager only)")
            .WithDescription("Updates an existing import service config. IsActive cannot be updated via this endpoint. Requires Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateImportServiceConfigRequestDto(
    decimal BaseShippingFee,
    string CountryCode,
    string CountryName,
    decimal ImportTaxPercentage,
    int EstimatedDeliveryDays);
