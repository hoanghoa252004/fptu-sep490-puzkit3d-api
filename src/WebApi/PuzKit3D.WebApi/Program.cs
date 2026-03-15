using PuzKit3D.Modules.Cart.Api;
using PuzKit3D.Modules.Cart.Application;
using PuzKit3D.Modules.Cart.Infrastructure;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.Modules.Catalog.Api;
using PuzKit3D.Modules.Catalog.Application;
using PuzKit3D.Modules.Catalog.Persistence;
using PuzKit3D.Modules.InStock.Api;
using PuzKit3D.Modules.InStock.Application;
using PuzKit3D.Modules.InStock.Infrastructure;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.Modules.Media.Infrastructure.DependencyInjection.Extensions;
using PuzKit3D.Modules.Notification.Api;
using PuzKit3D.Modules.Notification.Application;
using PuzKit3D.Modules.Notification.Application.Services;
using PuzKit3D.Modules.Notification.Infrastructure.DependencyInjection.Extensions;
using PuzKit3D.Modules.Notification.Infrastructure.Services;
using PuzKit3D.Modules.Partner.Api;
using PuzKit3D.Modules.Partner.Application;
using PuzKit3D.Modules.Partner.Persistence;
using PuzKit3D.Modules.Payment.Api;
using PuzKit3D.Modules.Payment.Application;
using PuzKit3D.Modules.Payment.Infrastructure;
using PuzKit3D.Modules.Payment.Persistence;
using PuzKit3D.Modules.User.Api;
using PuzKit3D.Modules.User.Application;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Application;
using PuzKit3D.SharedKernel.Infrastructure;
using PuzKit3D.WebApi.DependencyInjection.Extensions;
using PuzKit3D.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine(builder.Configuration["ConnectionStrings:DefaultConnection"]);
// Service:
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

// SharedKernel
builder.Services.AddSharedKernelInfrastructure(builder.Configuration);
builder.Services.AddSharedKernelApplication(
    new[]{
        InstockApplicationAssembly.Assembly,
        UserApplicationAssembly.Assembly,
        CatalogApplicationAssembly.Assembly,
        CartApplicationAssembly.Assembly,
        PartnerApplicationAssembly.Assembly,
        PaymentApplicationAssembly.Assembly,
        NotificationApplicationAssembly.Assembly,
        MediaApplicationAssembly.Assembly
    } 
);

// Add Endpoints from all Modules.XXXX.API assemblies:
builder.Services.AddEndpointsFromAssembly(
    new[]
    {
       InstockApiAssembly.Assembly,
       UserApiAssembly.Assembly,
       CatalogApiAssembly.Assembly,
       CartApiAssembly.Assembly,
       PartnerApiAssembly.Assembly,
       PaymentApiAssembly.Assembly,
       NotificationApiAssembly.Assembly,
       MediaApiAssembly.Assembly
    }
);

// Add Persistence services (DbContext, Repositories ):
builder.Services.AddInStockPersistence(builder.Configuration);
builder.Services.AddCatalogPersistence(builder.Configuration);
builder.Services.AddCartPersistence(builder.Configuration);
builder.Services.AddPartnerPersistence(builder.Configuration);
builder.Services.AddPaymentPersistence(builder.Configuration);

// Add Infrastructure services (Domain Event Handlers, Integration Event Handlers):
builder.Services.AddInStockInfrastructure();
builder.Services.AddCartInfrastructure();
builder.Services.AddPaymentInfrastructure();
builder.Services.AddNotificationInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddMediaInfrastructure(builder.Configuration, builder.Environment);

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var emailService = scope.ServiceProvider.GetRequiredService<AwsSesEmailService>();

//    await emailService.InitializeEmailTemplate();
//}

app.UseSwagger();
// Tự động sinh file JSON chứa thông tin API tại endpoint /swagger/v1/swagger.json
// Mô tả tất cả endpoints, parameters, responses theo chuẩn OpenAPI

app.UseSwaggerUI(options =>
{
    // Module-specific documents
    options.SwaggerEndpoint("/swagger/user/swagger.json", "1. User Module");
    options.SwaggerEndpoint("/swagger/cart/swagger.json", "2. Cart Module");
    options.SwaggerEndpoint("/swagger/catalog/swagger.json", "3. Catalog Module");
    options.SwaggerEndpoint("/swagger/instock/swagger.json", "4. Instock Module");
    options.SwaggerEndpoint("/swagger/partner/swagger.json", "5. Partner Module");
    options.SwaggerEndpoint("/swagger/payment/swagger.json", "6. Payment Module");
    options.SwaggerEndpoint("/swagger/notification/swagger.json", "7. Notification Module");
    options.SwaggerEndpoint("/swagger/media/swagger.json", "8. Media Module");
    // Main API document
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "All Modules");

    options.RoutePrefix = "swagger"; // Access at /swagger [default]
    options.DisplayRequestDuration(); // Show request duration
    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None); 
});


app.MapGet("/", () => "Welcome to PuzKit3D API").ExcludeFromDescription();

app.UseExceptionHandler();

app.UseCors("AllowAll");

// Authentication & Authorization - MUST be added before MapEndpoints()
app.UseAuthentication();
app.UseAuthorization();

// Map Endpoints:
app.MapEndpoints();

app.Run();


