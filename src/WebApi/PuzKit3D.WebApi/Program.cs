using PuzKit3D.Modules.Cart.Infrastructure;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.Modules.Catalog.Persistence;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Application;
using PuzKit3D.SharedKernel.Infrastructure;
using PuzKit3D.WebApi.DependencyInjection.Extensions;
using PuzKit3D.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Service:
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

// SharedKernel
builder.Services.AddSharedKernelInfrastructure(builder.Configuration);
builder.Services.AddSharedKernelApplication(
    new[]{
        PuzKit3D.Modules.InStock.Application.InstockApplicationAssembly.Assembly,
        PuzKit3D.Modules.User.Application.UserApplicationAssembly.Assembly,
        PuzKit3D.Modules.Catalog.Application.CatalogApplicationAssembly.Assembly,
        PuzKit3D.Modules.Cart.Application.CartApplicationAssembly.Assembly
    } 
);

// Add Endpoints from all Modules.XXXX.API assemblies:
builder.Services.AddEndpointsFromAssembly(
    new[]
    {
       PuzKit3D.Modules.InStock.Api.InstockApiAssembly.Assembly,
       PuzKit3D.Modules.User.Api.UserApiAssembly.Assembly,
       PuzKit3D.Modules.Catalog.Api.CatalogApiAssembly.Assembly,
       PuzKit3D.Modules.Cart.Api.CartApiAssembly.Assembly
    }
);

// Add Persistence services (DbContext, Repositories ):
builder.Services.AddInStockPersistence(builder.Configuration);
builder.Services.AddCatalogPersistence(builder.Configuration);
builder.Services.AddCartPersistence(builder.Configuration);

// Add Infrastructure services (Domain Event Handlers, Integration Event Handlers):
//builder.Services.AddCartInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // Tự động sinh file JSON chứa thông tin API tại endpoint /swagger/v1/swagger.json
    // Mô tả tất cả endpoints, parameters, responses theo chuẩn OpenAPI

    app.UseSwaggerUI(options =>
    {
        // Main API document
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "All Modules");

        // Module-specific documents
        options.SwaggerEndpoint("/swagger/user/swagger.json", "1. User Module");
        options.SwaggerEndpoint("/swagger/cart/swagger.json", "2. Cart Module");
        options.SwaggerEndpoint("/swagger/catalog/swagger.json", "3. Catalog Module");
        options.SwaggerEndpoint("/swagger/instock/swagger.json", "4. Instock Module");
        options.SwaggerEndpoint("/swagger/partner/swagger.json", "5. Partner Module");
        options.SwaggerEndpoint("/swagger/payment/swagger.json", "6. Payment Module");
        
        options.RoutePrefix = "swagger"; // Access at /swagger [default]
        options.DisplayRequestDuration(); // Show request duration
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); // Collapse all by default
    });
}

app.MapGet("/", () => "Welcome to PuzKit3D API").ExcludeFromDescription();

app.UseExceptionHandler();

// Authentication & Authorization - MUST be added before MapEndpoints()
app.UseAuthentication();
app.UseAuthorization();

// Map Endpoints:
app.MapEndpoints();

app.Run();


