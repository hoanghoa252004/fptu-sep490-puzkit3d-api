using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Commands.CreatePartnerProduct;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Partner.Api.PartnerProducts.CreatePartnerProduct;

internal sealed class CreatePartnerProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPartnerProductsGroup()
            .MapPost("/", async (
                [FromBody] CreatePartnerProductRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreatePartnerProductCommand(
                    request.PartnerId,
                    request.Name,
                    request.ReferencePrice,
                    request.Quantity,
                    request.ThumbnailUrl,
                    request.PreviewAsset,
                    request.Slug,
                    request.Description,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchCreated("GetPartnerProductById", id => new { id });
            })
            .WithName("CreatePartnerProduct")
            .WithSummary("Create a new partner product (Staff/Manager only)")
            .WithDescription("Creates a new partner product. Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreatePartnerProductRequestDto(
    Guid PartnerId,
    string Name,
    decimal ReferencePrice,
    int Quantity,
    string ThumbnailUrl,
    Dictionary<string, string> PreviewAsset,
    string Slug,
    string? Description,
    bool IsActive);
