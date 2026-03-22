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
                Title = "1. User Module",
                Version = "v1",
                Description = "APIs for user management and authentication"
            });

            options.SwaggerDoc("cart", new OpenApiInfo
            {
                Title = "2. Cart Module",
                Version = "v1",
                Description = "APIs for shopping cart management: INSTOCK_CART, PARTNER_CART"
            });

            options.SwaggerDoc("catalog", new OpenApiInfo
            {
                Title = "3. Catalog Module",
                Version = "v1",
                Description = "APIs for managing product catalog: TOPIC, MATERIAL, ASSEMBLY_METHOD, CAPABILITY"
            });

            options.SwaggerDoc("instock", new OpenApiInfo
            {
                Title = "4. Instock Module",
                Version = "v1",
                Description = "APIs for instock management: INSTOCK_PRODUCT, INSTOCK_ORDER, INSTOCK_INVENTORY, PRICE"
            });

            options.SwaggerDoc("partner", new OpenApiInfo
            {
                Title = "5. Partner Module",
                Version = "v1",
                Description = "APIs for partner management: PARTNER, PARTNER_PRODUCT, PARTNER_REQUEST, PARTNER_QUOTATION, PARTNER_ORDER"
            });

            options.SwaggerDoc("payment", new OpenApiInfo
            {
                Title = "6. Payment Module",
                Version = "v1",
                Description = "APIs for payment processing"
            });

            options.SwaggerDoc("notification", new OpenApiInfo
            {
                Title = "7. Notification Module",
                Version = "v1",
                Description = "APIs for notification processing"
            });

            options.SwaggerDoc("media", new OpenApiInfo
            {
                Title = "8. Media Module",
                Version = "v1",
                Description = "APIs for media processing"
            });

            options.SwaggerDoc("delivery", new OpenApiInfo
            {
                Title = "9. Delivery Module",
                Version = "v1",
                Description = "APIs for delivery processing"
            });

            options.SwaggerDoc("feedback", new OpenApiInfo
            {
                Title = "10. Feedback Module",
                Version = "v1",
                Description = "APIs for feedback processing"
            });

            options.SwaggerDoc("support-tickets", new OpenApiInfo
            {
                Title = "11. Support Ticket",
                Version = "v1",
                Description = "APIs for support ticket processing"
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

                    "partner" => routePath.StartsWith("api/partnerXXX"),

                    "payment" => routePath.StartsWith("api/payments")
                                || routePath.StartsWith("api/ipn")
                                || routePath.StartsWith("api/orders"),

                    "notification" => routePath.StartsWith("api/emails"),

                    "media" => routePath.StartsWith("api/uploads"),

                    "delivery" => routePath.StartsWith("api/delivery"),

                    "feedback" => routePath.Contains("feedback"),

                    "support-tickets" => routePath.StartsWith("api/support-tickets"),

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
