using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequests.Commands.CreateRequest;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.CustomDesign.Api.CustomDesignRequests.CreateRequest;

internal sealed class CreateCustomDesignRequest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCustomDesignRequestGroup()
            .MapPost("/", async (
                [FromBody] CreateCustomDesignRequestRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                // Convert sketches array to comma-separated string
                var sketchesString = request.Sketches?.Count > 0 
                    ? string.Join(",", request.Sketches.Where(s => !string.IsNullOrWhiteSpace(s))) 
                    : null;

                var command = new CreateCustomDesignRequestCommand(
                    request.CustomDesignRequirementId,
                    request.DesiredLengthMm,
                    request.DesiredWidthMm,
                    request.DesiredHeightMm,
                    sketchesString,
                    request.CustomerPrompt,
                    request.DesiredDeliveryDate,
                    request.DesiredQuantity,
                    request.TargetBudget,
                    request.Type);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk(id => Results.Created($"/api/custom-design-requests/{id}", id));
            })
            .WithName("CreateCustomDesignRequest")
            .WithSummary("Create a new custom design request")
            .WithDescription("Creates a new custom design request. Code is auto-generated (CDR001, CDR002, ...). CustomerId is taken from JWT token. Status is automatically set to 'Submitted'.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateCustomDesignRequestRequestDto(
string Type,
Guid CustomDesignRequirementId,
decimal DesiredLengthMm,
decimal DesiredWidthMm,
decimal DesiredHeightMm,
List<string>? Sketches,
string? CustomerPrompt,
DateTime DesiredDeliveryDate,
int DesiredQuantity,
decimal TargetBudget);

