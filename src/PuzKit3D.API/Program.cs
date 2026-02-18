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

var app = builder.Build();

app.MapGet("/", () => "Welcome to PuzKit3D API");

app.UseExceptionHandler();

// Map Endpoints:
app.MapEndpoints();

app.Run();

