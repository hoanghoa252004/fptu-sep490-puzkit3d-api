using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Commands.UpdatePartnerProductQuotationStatus;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Partner.Api.PartnerProductQuotations.UpdatePartnerProductQuotationStatus;

internal sealed class UpdatePartnerProductQuotationStatus : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductQuotationsGroup()
            .MapPut("/{id}/status", async (
                [FromRoute] Guid id,
                [FromBody] UpdatePartnerProductQuotationStatusRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdatePartnerProductQuotationStatusCommand(
                    id,
                    request.NewStatus,
                    request.Note);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk(success => Results.Ok(new { quotationId = success }));
            })
            .WithName("UpdatePartnerProductQuotationStatus")
            .WithSummary("Update status of a partner product quotation")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager, Roles.Customer))
            .Produces<object>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    public sealed record UpdatePartnerProductQuotationStatusRequestDto(
        PartnerProductQuotationStatus NewStatus,
        string? Note = null);
}
