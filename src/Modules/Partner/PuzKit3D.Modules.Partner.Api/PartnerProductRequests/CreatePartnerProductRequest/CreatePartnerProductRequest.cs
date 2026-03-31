using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Commands.CreatePartnerProductRequest;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

using AppItemDto = PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Commands.CreatePartnerProductRequest.CreatePartnerProductRequestItemRequestDto;

namespace PuzKit3D.Modules.Partner.Api.PartnerProductRequests.CreatePartnerProductRequest;

internal sealed class CreatePartnerProductRequest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductRequestsGroup()
            .MapPost("/", async (
                [FromBody] CreatePartnerProductRequestRequestDto request,
                ISender sender,
                IHttpContextAccessor httpContextAccessor,
                CancellationToken cancellationToken) =>
            {
                var customerId = httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?
                    .Value;
                if (string.IsNullOrEmpty(customerId) || !Guid.TryParse(customerId, out var customerGuid))
                {
                    return Results.Unauthorized();
                }

                var command = new CreatePartnerProductRequestCommand(
                    customerGuid,
                    request.PartnerId,
                    request.DesiredDeliveryDate,
                    request.Items
                        .ConvertAll(i => new AppItemDto(
                            i.PartnerProductId,
                            i.Quantity)));

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
    List<CreatePartnerProductRequestItemRequestDto> Items);

internal sealed record CreatePartnerProductRequestItemRequestDto(
    Guid PartnerProductId,
    int Quantity);
