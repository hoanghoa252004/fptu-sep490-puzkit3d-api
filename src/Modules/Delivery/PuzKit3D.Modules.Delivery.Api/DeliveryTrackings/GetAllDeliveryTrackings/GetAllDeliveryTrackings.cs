using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetDeliveryTrackings;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using PuzKit3D.SharedKernel.Application.Authorization;

namespace PuzKit3D.Modules.Delivery.Api.DeliveryTrackings.GetAllDeliveryTrackings;

internal sealed class GetAllDeliveryTrackings : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDeliveryGroup()
            .MapGet("/", async (
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10,
                [FromQuery] string? status = null,
                ISender sender = default!,
                CancellationToken cancellationToken = default) =>
            {
                var query = new GetDeliveryTrackingsQuery(pageNumber, pageSize, status);
                var result = await sender.Send(query, cancellationToken);
                return result.MatchOk();
            })
            .WithName("GetAllDeliveryTrackings")
            .RequireAuthorization(builder => builder.RequireRole(Roles.Staff, Roles.BusinessManager))
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

