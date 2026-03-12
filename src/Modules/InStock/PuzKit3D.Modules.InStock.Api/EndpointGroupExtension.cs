using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.SharedKernel.Api.Endpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.InStock.Api;

public static class EndpointGroupExtension
{
    public static RouteGroupBuilder MapProductsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/instock-products")
            .WithTags("Products");
    }

    public static RouteGroupBuilder MapPartsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/instock-products/{{productId:guid}}/parts")
            .WithTags("Parts");
    }

    public static RouteGroupBuilder MapPiecesGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/instock-products/{{productId:guid}}/parts/{{partId:guid}}/pieces")
            .WithTags("Pieces");
    }

    public static RouteGroupBuilder MapVariantsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/instock-products/{{productId:guid}}/variants")
            .WithTags("Product Variants");
    }

    public static RouteGroupBuilder MapPricesGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/instock-prices")
            .WithTags("Prices");
    }

    public static RouteGroupBuilder MapPriceDetailsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/instock-price-details")
            .WithTags("Price Details");
    }


    public static RouteGroupBuilder MapInventoryGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/instock-products/{{productId:guid}}/variants/{{variantId:guid}}/inventory")
            .WithTags("Inventory");
    }

    public static RouteGroupBuilder MapOrdersGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/instock-orders")
            .WithTags("InStock Orders");
    }
}
