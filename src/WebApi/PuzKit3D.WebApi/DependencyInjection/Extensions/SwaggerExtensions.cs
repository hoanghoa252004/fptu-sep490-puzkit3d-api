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

                    "instock" => routePath.StartsWith("api/products") 
                                || routePath.StartsWith("api/orders"),

                    "user" => routePath.StartsWith("api/users") 
                                || routePath.StartsWith("api/auth"),

                    "partner" => routePath.StartsWith("api/partnerXXX"),

                    "payment" => routePath.StartsWith("api/payment"),
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
