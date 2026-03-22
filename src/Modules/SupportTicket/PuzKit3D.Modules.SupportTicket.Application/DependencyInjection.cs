using Microsoft.Extensions.DependencyInjection;

namespace PuzKit3D.Modules.SupportTicket.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddSupportTicketApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
