using PuzKit3D.API.DependencyInjection.Extensions;
using PuzKit3D.API.Middleware;
using PuzKit3D.Application.DependencyInjection.Extensions;
using PuzKit3D.Presentation.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Configuration:
builder.Services.AddApplicationServices();
builder.Services.AddPresentationServices();


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

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

// Map Endpoints:
app.MapEndpoints();

app.Run();

