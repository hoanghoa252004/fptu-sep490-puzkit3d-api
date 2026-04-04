using PuzKit3D.Modules.Cart.Api;
using PuzKit3D.Modules.Cart.Application;
using PuzKit3D.Modules.Cart.Infrastructure;
using PuzKit3D.Modules.Cart.Persistence;
using PuzKit3D.Modules.Catalog.Api;
using PuzKit3D.Modules.Catalog.Application;
using PuzKit3D.Modules.Catalog.Persistence;
using PuzKit3D.Modules.CustomDesign.Api;
using PuzKit3D.Modules.CustomDesign.Application;
using PuzKit3D.Modules.CustomDesign.Infrastructure;
using PuzKit3D.Modules.CustomDesign.Persistence;
using PuzKit3D.Modules.Delivery.Api;
using PuzKit3D.Modules.Delivery.Application;
using PuzKit3D.Modules.Delivery.Infrastructure;
using PuzKit3D.Modules.Delivery.Infrastructure.DependencyInjection.Extensions;
using PuzKit3D.Modules.Delivery.Persistence;
using PuzKit3D.Modules.Feedback.Api;
using PuzKit3D.Modules.Feedback.Application;
using PuzKit3D.Modules.Feedback.Infrastructure;
using PuzKit3D.Modules.Feedback.Persistence;
using PuzKit3D.Modules.InStock.Api;
using PuzKit3D.Modules.InStock.Application;
using PuzKit3D.Modules.InStock.Infrastructure;
using PuzKit3D.Modules.InStock.Persistence;
using PuzKit3D.Modules.Media.Api;
using PuzKit3D.Modules.Media.Application;
using PuzKit3D.Modules.Media.Infrastructure.DependencyInjection.Extensions;
using PuzKit3D.Modules.Notification.Api;
using PuzKit3D.Modules.Notification.Application;
using PuzKit3D.Modules.Notification.Application.Services;
using PuzKit3D.Modules.Notification.Infrastructure.DependencyInjection.Extensions;
using PuzKit3D.Modules.Notification.Infrastructure.Services;
using PuzKit3D.Modules.Partner.Api;
using PuzKit3D.Modules.Partner.Application;
using PuzKit3D.Modules.Partner.Infrastructure;
using PuzKit3D.Modules.Partner.Persistence;
using PuzKit3D.Modules.Payment.Api;
using PuzKit3D.Modules.Payment.Application;
using PuzKit3D.Modules.Payment.Infrastructure;
using PuzKit3D.Modules.Payment.Persistence;
using PuzKit3D.Modules.SupportTicket.Api;
using PuzKit3D.Modules.SupportTicket.Application;
using PuzKit3D.Modules.SupportTicket.Infrastructure;
using PuzKit3D.Modules.SupportTicket.Persistence;
using PuzKit3D.Modules.User.Api;
using PuzKit3D.Modules.User.Application;
using PuzKit3D.Modules.Wallet.Api;
using PuzKit3D.Modules.Wallet.Application;
using PuzKit3D.Modules.Wallet.Infrastructure.DependencyInjection.Extensions;
using PuzKit3D.Modules.Wallet.Persistence;
using PuzKit3D.SharedKernel.Api.Endpoint;
using PuzKit3D.SharedKernel.Application;
using PuzKit3D.SharedKernel.Infrastructure;
using PuzKit3D.WebApi.BackgroundJobs;
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
        MediaApplicationAssembly.Assembly,
        DeliveryApplicationAssembly.Assembly,
        FeedbackApplicationAssembly.Assembly,
        SupportTicketApplicationAssembly.Assembly,
        WalletApplicationAssembly.Assembly,
        CustomDesignApplicationAssembly.Assembly,
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
       MediaApiAssembly.Assembly,
       DeliveryApiAssembly.Assembly,
       FeedbackApiAssembly.Assembly,
       SupportTicketApiAssembly.Assembly,
       WalletApiAssembly.Assembly,
       CustomDesignApiAssembly.Assembly,
    }
);

// Add Persistence services (DbContext, Repositories ):
builder.Services.AddInStockPersistence(builder.Configuration);
builder.Services.AddCatalogPersistence(builder.Configuration);
builder.Services.AddCartPersistence(builder.Configuration);
builder.Services.AddPartnerPersistence(builder.Configuration);
builder.Services.AddPaymentPersistence(builder.Configuration);
builder.Services.AddFeedbackPersistence(builder.Configuration);
builder.Services.AddSupportTicketPersistence(builder.Configuration);
builder.Services.AddDeliveryPersistence(builder.Configuration);
builder.Services.AddWalletPersistence(builder.Configuration);
builder.Services.AddCustomDesignPersistence(builder.Configuration);

// Add Infrastructure services (Domain Event Handlers, Integration Event Handlers):
builder.Services.AddInStockInfrastructure(builder.Configuration);
builder.Services.AddCartInfrastructure(builder.Configuration);
builder.Services.AddPaymentInfrastructure();
builder.Services.AddFeedbackInfrastructure();
builder.Services.AddNotificationInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddMediaInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddDeliveryInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddSupportTicketInfrastructure();
builder.Services.AddWalletInfrastructure(builder.Configuration); 
builder.Services.AddPartnerInfrastructure();
builder.Services.AddCustomDesignInfrastructure(builder.Configuration);

// Add Background Services (Cronjobs)
builder.Services.AddHostedService<PaymentExpiryCheckService>();
builder.Services.AddHostedService<OrderCompletionCheckService>();
builder.Services.AddHostedService<DeliveryTrackingUpdateStatusService>();

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
    options.SwaggerEndpoint("/swagger/user/swagger.json", "User API");
    options.SwaggerEndpoint("/swagger/cart/swagger.json", "Cart API");
    options.SwaggerEndpoint("/swagger/catalog/swagger.json", "Catalog API");
    options.SwaggerEndpoint("/swagger/instock/swagger.json", "Instock API");
    options.SwaggerEndpoint("/swagger/partner/swagger.json", "Partner API");
    options.SwaggerEndpoint("/swagger/payment/swagger.json", "Payment API");
    options.SwaggerEndpoint("/swagger/media/swagger.json", "Media Storage API");
    options.SwaggerEndpoint("/swagger/delivery/swagger.json", "Delivery Tracking API");
    options.SwaggerEndpoint("/swagger/after-sale/swagger.json", "After Sale API");
    options.SwaggerEndpoint("/swagger/config/swagger.json", "Business Rule Config API");
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


