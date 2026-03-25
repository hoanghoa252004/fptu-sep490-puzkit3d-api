using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.Partners.Commands.UpdatePartner;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Partner.Api.Partners.UpdatePartner;

internal sealed class UpdatePartner : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnersGroup()
            .MapPut("/{id:guid}", async (
                Guid id,
                [FromBody] UpdatePartnerRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdatePartnerCommand(
                    id,
                    request.ImportServiceConfigId,
                    request.Name,
                    request.ContactEmail,
                    request.ContactPhone,
                    request.Address,
                    request.Slug,
                    request.Description);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchNoContent();
            })
            .WithName("UpdatePartner")
            .WithSummary("Update a partner (Staff/Manager only)")
            .WithDescription("Updates an existing partner with new details. IsActive cannot be updated via this endpoint. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record UpdatePartnerRequestDto(
    Guid ImportServiceConfigId,
    string Name,
    string ContactEmail,
    string ContactPhone,
    string Address,
    string Slug,
    string? Description = null);
