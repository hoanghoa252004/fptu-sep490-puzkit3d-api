using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Commands.UpdateRequirement;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.CustomDesign.Api.CustomDesignRequirements.UpdateRequirement;

internal sealed class UpdateCustomDesignRequirement : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCustomDesignRequirementGroup()
            .MapPut("/{id:guid}", async (
                Guid id,
                [FromBody] UpdateCustomDesignRequirementRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateCustomDesignRequirementCommand(
                    id,
                    request.TopicId,
                    request.MaterialId,
                    request.AssemblyMethodId,
                    request.Difficulty,
                    request.MinPartQuantity,
                    request.MaxPartQuantity,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk();
            })
            .WithName("UpdateCustomDesignRequirement")
            .WithSummary("Update custom design requirement (Staff/Manager only)")
            .WithDescription("Updates a custom design requirement. Only provided fields will be updated. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdateCustomDesignRequirementRequestDto(
    Guid? TopicId = null,
    Guid? MaterialId = null,
    Guid? AssemblyMethodId = null,
    string? Difficulty = null,
    int? MinPartQuantity = null,
    int? MaxPartQuantity = null,
    bool? IsActive = null);
