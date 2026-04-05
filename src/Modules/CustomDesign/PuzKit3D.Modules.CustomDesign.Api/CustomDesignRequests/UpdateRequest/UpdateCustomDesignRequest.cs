using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequests.Commands.UpdateRequest;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.CustomDesign.Api.CustomDesignRequests.UpdateRequest;

internal sealed class UpdateCustomDesignRequest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCustomDesignRequestGroup()
            .MapPut("/{id}", async (
                Guid id,
                [FromBody] UpdateCustomDesignRequestRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                // Convert sketches array to comma-separated string if provided
                var sketchesString = request.Sketches?.Count > 0
                    ? string.Join(",", request.Sketches.Where(s => !string.IsNullOrWhiteSpace(s)))
                    : null;

                var command = new UpdateCustomDesignRequestCommand(
                    id,
                    request.DesiredLengthMm,
                    request.DesiredWidthMm,
                    request.DesiredHeightMm,
                    sketchesString,
                    request.CustomerPrompt,
                    request.DesiredDeliveryDate,
                    request.DesiredQuantity,
                    request.TargetBudget,
                    request.Status,
                    request.Note);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdateCustomDesignRequest")
            .WithSummary("Update a custom design request")
            .WithDescription("Updates an existing custom design request. Only Customer and Staff roles are allowed. Cannot update Code, CustomerId, Type, or UsedSupportConceptDesignTime fields.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer, Roles.Staff))
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateCustomDesignRequestRequestDto(
decimal? DesiredLengthMm,
decimal? DesiredWidthMm,
decimal? DesiredHeightMm,
List<string>? Sketches,
string? CustomerPrompt,
DateTime? DesiredDeliveryDate,
int? DesiredQuantity,
decimal? TargetBudget,
string? Status,
string? Note);

