using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PuzKit3D.SharedKernel.Api.Endpoint;

namespace PuzKit3D.Modules.Partner.Api;

public static class EndpointGroupExtension
{
    public static RouteGroupBuilder MapPartnerProductsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/partner-products")
            .WithTags("Partner Products");
    }

    public static RouteGroupBuilder MapPartnersGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/partners")
            .WithTags("Partners");
    }

    public static RouteGroupBuilder MapImportServiceConfigsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/import-service-configs")
            .WithTags("Import Service Configs");
    }

    public static RouteGroupBuilder MapPartnerProductRequestsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/partner-product-requests")
            .WithTags("Partner Product Requests");
    }

    public static RouteGroupBuilder MapPartnerProductQuotationsGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/partner-quotations")
            .WithTags("Partner Product Quotations");
    }

    public static RouteGroupBuilder MapPartnerProductOrdersGroup(this IEndpointRouteBuilder app)
    {
        return app.MapGroup($"{ApiRoutes.ApiPrefix}/partner-orders")
            .WithTags("Partner Product Orders");
    }
}
