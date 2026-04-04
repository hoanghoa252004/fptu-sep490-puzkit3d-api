using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Commands.CreateRequirement;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.CustomDesign.Api.CustomDesignRequirements.CreateRequirement;

internal sealed class CreateCustomDesignRequirement : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapCustomDesignRequirementGroup()
            .MapPost("/", async (
                [FromBody] CreateCustomDesignRequirementRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateCustomDesignRequirementCommand(
                    request.TopicId,
                    request.MaterialId,
                    request.AssemblyMethodId,
                    request.Difficulty,
                    request.MinPartQuantity,
                    request.MaxPartQuantity,
                    request.CapabilityIds);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk(id => Results.Created($"/api/custom-design-requirements/{id}", id));
            })
            .WithName("CreateCustomDesignRequirement")
            .WithSummary("Create a new custom design requirement (Staff/Manager only)")
            .WithDescription("Creates a new custom design requirement. Code is auto-generated (CDR001, CDR002, ...). Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateCustomDesignRequirementRequestDto(
Guid TopicId,
Guid MaterialId,
Guid AssemblyMethodId,
string Difficulty,
int MinPartQuantity,
int MaxPartQuantity,
IEnumerable<Guid> CapabilityIds);
