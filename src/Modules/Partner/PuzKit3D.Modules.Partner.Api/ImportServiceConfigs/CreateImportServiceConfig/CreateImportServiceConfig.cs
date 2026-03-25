using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Commands.CreateImportServiceConfig;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Partner.Api.ImportServiceConfigs.CreateImportServiceConfig;

internal sealed class CreateImportServiceConfig : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapImportServiceConfigsGroup()
            .MapPost("/", async (
                [FromBody] CreateImportServiceConfigRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateImportServiceConfigCommand(
                    request.BaseShippingFee,
                    request.CountryCode,
                    request.CountryName,
                    request.ImportTaxPercentage);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetImportServiceConfigById", id => new { id });
            })
            .WithName("CreateImportServiceConfig")
            .WithSummary("Create a new import service config (Staff/Manager only)")
            .WithDescription("Creates a new import service config. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateImportServiceConfigRequestDto(
    decimal BaseShippingFee,
    string CountryCode,
    string CountryName,
    decimal ImportTaxPercentage);
