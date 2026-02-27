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
        PuzKit3D.Modules.Catalog.Application.CatalogApplicationAssembly.Assembly
    } 
);

// Add Endpoints from all Modules.XXXX.API assemblies:
builder.Services.AddEndpointsFromAssembly(
    new[]
    {
       PuzKit3D.Modules.InStock.Api.InstockApiAssembly.Assembly,
       PuzKit3D.Modules.User.Api.UserApiAssembly.Assembly,
       PuzKit3D.Modules.Catalog.Api.CatalogApiAssembly.Assembly
    }
);

// Add Persistence services (DbContext, Repositories ):
builder.Services.AddInStockPersistence(builder.Configuration);
builder.Services.AddCatalogPersistence(builder.Configuration);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // Tự động sinh file JSON chứa thông tin API tại endpoint /swagger/v1/swagger.json
    // Mô tả tất cả endpoints, parameters, responses theo chuẩn OpenAPI

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "PuzKit3D API v1"); // [default]
        options.RoutePrefix = "swagger"; // Access at /swagger [default]
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


