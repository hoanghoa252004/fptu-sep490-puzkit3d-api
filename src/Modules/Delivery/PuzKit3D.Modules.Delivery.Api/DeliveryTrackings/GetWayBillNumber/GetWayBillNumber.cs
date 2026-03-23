using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Queries.GetWayBillNumber;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Delivery.Api.DeliveryTrackings.GetWayBillNumber;

internal sealed class GetWaybillNumber : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDeliveryGroup()
            .MapGet("/{deliveryTrackingId}/waybill-number", async (
                [FromRoute] Guid deliveryTrackingId,
                ISender _sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetWayBillNumberQuery(deliveryTrackingId);
                var result = await _sender.Send(query, cancellationToken);
                return result.MatchOk();
            })
            .WithName("GetWaybillNumber")
            .WithDescription("Get waybill number URL for an InStock Order")
            .Produces<object>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

