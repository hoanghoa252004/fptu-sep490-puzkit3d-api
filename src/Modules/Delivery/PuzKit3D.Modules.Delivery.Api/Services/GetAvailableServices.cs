using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.Modules.Delivery.Api;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;

namespace PuzKit3D.Modules.Delivery.Api.Services;

internal sealed class GetAvailableServices : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapShippingGroup()
            .MapPost("/available-services", async (GetAvailableServicesRequest request, IDeliveryService deliveryService) =>
            {
                var result = await deliveryService.GetAvailableServicesAsync(request.FromDistrict, request.ToDistrict);
                return result.MatchOk();
            })
            .WithName("GetAvailableServices")
            .WithDescription("Get available shipping services from GHN")
            .AllowAnonymous()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

public record GetAvailableServicesRequest(int FromDistrict, int ToDistrict);
