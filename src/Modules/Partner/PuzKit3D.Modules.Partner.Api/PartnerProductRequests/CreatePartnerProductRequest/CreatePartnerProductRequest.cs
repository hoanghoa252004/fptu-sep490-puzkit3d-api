using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Commands.CreatePartnerProductRequest;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Partner.Api.PartnerProductRequests.CreatePartnerProductRequest;

internal sealed class CreatePartnerProductRequest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiRoutes.ApiPrefix}/partner-request")
            .WithTags("Partner Requests")
            .MapPost("/", async (
                [FromBody] CreatePartnerProductRequestRequestDto request,
                ISender sender,
                IHttpContextAccessor httpContextAccessor,
                CancellationToken cancellationToken) =>
            {
                var customerId = httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
                if (string.IsNullOrEmpty(customerId) || !Guid.TryParse(customerId, out var customerGuid))
                {
                    return Results.Unauthorized();
                }

                var command = new CreatePartnerProductRequestCommand(
                    customerGuid,
                    request.PartnerId,
                    request.DesiredDeliveryDate,
                    request.Items
                        .ConvertAll(i => new Application.UseCases.PartnerProductRequests.Commands.CreatePartnerProductRequest.CreatePartnerProductRequestItemDto(
                            i.PartnerProductId,
                            i.Quantity)),
                    request.Note);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetPartnerProductRequestById", id => new { id });
            })
            .WithName("CreatePartnerProductRequest")
            .WithSummary("Create a new partner product request (Customer only)")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Customer))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreatePartnerProductRequestRequestDto(
    Guid PartnerId,
    DateTime DesiredDeliveryDate,
    List<CreatePartnerProductRequestItemDto> Items,
    string? Note = null);

internal sealed record CreatePartnerProductRequestItemDto(
    Guid PartnerProductId,
    int Quantity);
