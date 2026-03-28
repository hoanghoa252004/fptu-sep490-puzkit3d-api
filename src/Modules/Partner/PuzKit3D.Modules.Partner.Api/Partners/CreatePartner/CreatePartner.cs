using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.Partners.Commands.CreatePartner;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Partner.Api.Partners.CreatePartner;

internal sealed class CreatePartner : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnersGroup()
            .MapPost("/", async (
                [FromBody] CreatePartnerRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreatePartnerCommand(
                    request.Name,
                    request.ContactEmail,
                    request.ContactPhone,
                    request.Address,
                    request.Slug,
                    request.ImportServiceConfigId,
                    request.Description);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetPartnerById", id => new { id });
            })
            .WithName("CreatePartner")
            .WithSummary("Create a new partner (Manager only)")
            .WithDescription("Creates a new partner. Requires Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreatePartnerRequestDto(
    string Name,
    string ContactEmail,
    string ContactPhone,
    string Address,
    string Slug,
    Guid ImportServiceConfigId,
    string? Description = null);
