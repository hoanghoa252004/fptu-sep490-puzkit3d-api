using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Commands.CreateInstockProduct;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.InStock.Api.InstockProducts.CreateInstockProduct;

internal sealed class CreateInstockProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapProductsGroup()
            .MapPost("/", async (
                [FromBody] CreateInstockProductRequestDto request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateInstockProductCommand(
                    request.Slug,
                    request.Name,
                    request.TotalPieceCount,
                    request.DifficultLevel,
                    request.EstimatedBuildTime,
                    request.ThumbnailUrl,
                    request.PreviewAsset,
                    request.TopicId,
                    request.AssemblyMethodId,
                    request.CapabilityId,
                    request.MaterialId,
                    request.Description,
                    request.IsActive);

                var result = await sender.Send(command, cancellationToken);

                return result.MatchOk(id => Results.Created($"/api/instock-products/{id}", id));
            })
            .WithName("CreateInstockProduct")
            .WithSummary("Create a new instock product (Staff/Manager only)")
            .WithDescription("Creates a new instock product. Code is auto-generated (INPxxx). Requires Staff or Manager role.")
            .RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

internal sealed record CreateInstockProductRequestDto(
string Slug,
string Name,
int TotalPieceCount,
string DifficultLevel,
int EstimatedBuildTime,
string ThumbnailUrl,
Dictionary<string, string> PreviewAsset,
Guid TopicId,
Guid AssemblyMethodId,
Guid CapabilityId,
Guid MaterialId,
string? Description,
bool IsActive);
