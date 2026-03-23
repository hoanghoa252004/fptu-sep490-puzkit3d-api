using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Delivery.Api.DeliveryTrackings.GetDeliveryTrackingById;

internal sealed class GetDeliveryTrackingById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGroup("/api/delivery-trackings")
            .WithTags("Delivery Trackings")
            .MapGet("/{id}", HandleGetById)
            .WithName("GetDeliveryTrackingById")
            .WithOpenApi()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> HandleGetById(
        [FromRoute] Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetDeliveryTrackingByIdQuery(id);
        var result = await sender.Send(query, cancellationToken);

        return result.MatchOk();
    }
}

