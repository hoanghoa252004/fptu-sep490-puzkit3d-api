using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.Contract.User;
using PuzKit3D.Modules.Partner.Infrastructure.IntegrationEventHandlers.Users;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Partner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPartnerInfrastructure(
        this IServiceCollection services)
    {
        //// User events
        services.AddScoped<IIntegrationEventHandler<UserEmailConfirmedIntegrationEvent>,
            UserEmailConfirmedIntegrationEventHandler>();

        services.AddScoped<IIntegrationEventHandler<UserUpdatedIntegrationEvent>,
            UserUpdatedIntegrationEventHandler>();
        return services;
    }
}
