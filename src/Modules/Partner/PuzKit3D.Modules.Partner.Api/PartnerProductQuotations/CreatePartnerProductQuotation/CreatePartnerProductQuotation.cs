using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Commands.CreatePartnerProductQuotation;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Partner.Api.PartnerProductQuotations.CreatePartnerProductQuotation;

internal sealed class CreatePartnerProductQuotation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductQuotationsGroup()
            .MapPost("/", async (
                [FromBody] CreatePartnerProductQuotationRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var items = request.Items?
                    .ConvertAll(i => new CreatePartnerProductQuotationItemDto(
                        i.PartnerProductId,
                        i.CustomUnitPrice));

                var command = new CreatePartnerProductQuotationCommand(
                    request.PartnerProductRequestId,
                    request.PartnerId,
                    request.ExpectedDeliveryDate,
                    items);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetPartnerProductQuotationById", id => new { id });
            })
            .WithName("CreatePartnerProductQuotation")
            .WithSummary("Create a new partner product quotation (Manager only)")
            .RequireAuthorization(policy => policy.RequireRole(Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    public sealed record CreatePartnerProductQuotationRequestDto(
        Guid PartnerProductRequestId,
        Guid PartnerId,
        DateTime ExpectedDeliveryDate,
        List<CreatePartnerProductQuotationItemRequestDto>? Items = null);

    public sealed record CreatePartnerProductQuotationItemRequestDto(
        Guid PartnerProductId,
        decimal? CustomUnitPrice = null);
}
