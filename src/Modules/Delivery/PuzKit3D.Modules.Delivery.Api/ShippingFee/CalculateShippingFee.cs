using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.Modules.Delivery.Api;
using PuzKit3D.Modules.Delivery.Application.DTOs;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Api.Results.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PuzKit3D.Modules.Delivery.Api.ShippingFee;

internal sealed class CalculateShippingFee : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDeliveryGroup()
            .MapGet("/shipping-fee", async (
                string provinceName, 
                string districtName,
                string wardName,
                IDeliveryService deliveryService) =>
            {
                var result = await deliveryService.CalculateShippingFeeByLocationAsync(new CalculateShippingFeeByLocationRequest()
                {
                    ProvinceName = provinceName,
                    DistrictName = districtName,
                    WardName = wardName
                });
                if (!result.IsSuccess)
                    return Results.BadRequest(result.Error);
                
                return Results.Ok(result.Value);
            })
            .WithName("CalculateShippingFeeByLocation")
            .WithDescription("Calculate shipping fee by province, district, and ward names")
            .AllowAnonymous()
            .Produces<int>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}