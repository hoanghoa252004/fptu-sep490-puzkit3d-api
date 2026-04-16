using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Commands.UpdateFormulaValueValidation;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.FormulaValueValidations.UpdateFormulaValueValidation;

internal sealed class UpdateFormulaValueValidation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFormulaValueValidationsGroup()
            .MapPut("/{id}", async (
                Guid id,
                [FromBody] UpdateFormulaValueValidationRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateFormulaValueValidationCommand(
                    id,
                    request.MinValue,
                    request.MaxValue,
                    request.Output);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdateFormulaValueValidation")
            .WithSummary("Update a formula value validation (Staff/Manager only)")
            .WithDescription("Updates an existing formula value validation. All fields are optional. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateFormulaValueValidationRequestDto(
    decimal? MinValue = null,
    decimal? MaxValue = null,
    string? Output = null);
