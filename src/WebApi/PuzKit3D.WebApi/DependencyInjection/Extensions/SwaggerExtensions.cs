using Microsoft.OpenApi.Models;

namespace PuzKit3D.WebApi.DependencyInjection.Extensions;

internal static class SwaggerExtensions
{
    internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PuzKit3D API",
                Version = "v1",
                Description = "Complete API documentation for all modules"
            });

            // Module-specific documents
            options.SwaggerDoc("user", new OpenApiInfo
            {
                Title = "User API",
                Version = "v1",
                Description = "APIs for user management and authentication"
            });

            options.SwaggerDoc("cart", new OpenApiInfo
            {
                Title = "Cart API",
                Version = "v1",
                Description = "APIs for shopping cart management: INSTOCK_CART, PARTNER_CART"
            });

            options.SwaggerDoc("catalog", new OpenApiInfo
            {
                Title = "Catalog API",
                Version = "v1",
                Description = "APIs for managing product catalog: TOPIC, MATERIAL, ASSEMBLY_METHOD, CAPABILITY"
            });

            options.SwaggerDoc("instock", new OpenApiInfo
            {
                Title = "Instock API",
                Version = "v1",
                Description = "APIs for instock management: INSTOCK_PRODUCT, INSTOCK_ORDER, INSTOCK_INVENTORY, PRICE"
            });

            options.SwaggerDoc("partner", new OpenApiInfo
            {
                Title = "Partner API",
                Version = "v1",
                Description = "APIs for partner management: PARTNER, PARTNER_PRODUCT, PARTNER_REQUEST, PARTNER_QUOTATION, PARTNER_ORDER"
            });

            options.SwaggerDoc("payment", new OpenApiInfo
            {
                Title = "Payment API",
                Version = "v1",
                Description = "APIs for payment processing"
            });

            options.SwaggerDoc("media", new OpenApiInfo
            {
                Title = "Media Storage API",
                Version = "v1",
                Description = "APIs for media processing"
            });

            options.SwaggerDoc("delivery", new OpenApiInfo
            {
                Title = "Delivery Tracking API",
                Version = "v1",
                Description = "APIs for delivery processing"
            });

            options.SwaggerDoc("after-sale", new OpenApiInfo
            {
                Title = "After Sale API",
                Version = "v1",
                Description = "APIs for feedback, support ticket processing"
            });



            // Group endpoints by module based on route prefix
            options.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (docName == "v1")
                    return true; // Include all endpoints in the main document

                // Get the relative path
                var routePath = apiDesc.RelativePath?.ToLower() ?? string.Empty;

                return docName switch
                {
                    "catalog" => routePath.StartsWith("api/topics") 
                                || routePath.StartsWith("api/assembly-methods")
                                || routePath.StartsWith("api/capabilities")
                                || routePath.StartsWith("api/materials"),

                    "cart" => routePath.StartsWith("api/instock-cart")
                                || routePath.StartsWith("api/partner-cart"),

                    "instock" => routePath.StartsWith("api/instock-products") 
                                || routePath.StartsWith("api/instock-prices")
                                || routePath.StartsWith("api/instock-price-details")
                                || routePath.StartsWith("api/instock-orders"),

                    "user" => routePath.StartsWith("api/users") 
                                || routePath.StartsWith("api/auth")
                                || routePath.StartsWith("api/profile"),

                    "partner" => routePath.StartsWith("api/partner-products") 
                                || routePath.StartsWith("api/import-service-configs")
                                || routePath.StartsWith("api/partners")
                                || routePath.StartsWith("api/partner-requests")
                                || routePath.StartsWith("api/partner-quotations")
                                || routePath.StartsWith("api/partner-orders"),

                    "payment" => routePath.Contains("payments")
                                || routePath.StartsWith("api/ipn"),

                    "media" => routePath.StartsWith("api/uploads"),

                    "delivery" => routePath.Contains("delivery"),

                    "after-sale" => routePath.Contains("feedback")
                                || routePath.StartsWith("api/support-tickets"),

                    _ => false
                };
            });

            // Add JWT Authentication
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. Enter your token in the text input below.\n\nExample: '12345abcdef'"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
