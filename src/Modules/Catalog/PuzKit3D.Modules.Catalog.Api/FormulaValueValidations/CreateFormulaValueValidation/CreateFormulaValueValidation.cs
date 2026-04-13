using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Catalog.Application.UseCases.FormulaValueValidations.Commands.CreateFormulaValueValidation;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Catalog.Api.FormulaValueValidations.CreateFormulaValueValidation;

internal sealed class CreateFormulaValueValidation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapFormulaValueValidationsGroup()
            .MapPost("/", async (
                [FromBody] CreateFormulaValueValidationRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateFormulaValueValidationCommand(
                    request.FormulaId,
                    request.MinValue,
                    request.MaxValue,
                    request.Output);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetFormulaValueValidationById", id => new { id });
            })
            .WithName("CreateFormulaValueValidation")
            .WithSummary("Create a new formula value validation (Staff/Manager only)")
            .WithDescription("Creates a new formula value validation with min/max value range and output mapping. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateFormulaValueValidationRequestDto(
    Guid FormulaId,
    decimal MinValue,
    decimal MaxValue,
    string Output);
