using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Presentation.Endpoints.Group;

public static class EndpointGroup
{
    private static readonly string API_PREFIX = "/api";
    internal static RouteGroupBuilder MapProductsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{API_PREFIX}/products")
            .WithTags("Products");
    }
    internal static RouteGroupBuilder MapOrdersGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{API_PREFIX}/orders")
            .WithTags("Orders");
    }
    internal static RouteGroupBuilder MapBrandsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{API_PREFIX}/brands")
            .WithTags("Brands");
    }
}
