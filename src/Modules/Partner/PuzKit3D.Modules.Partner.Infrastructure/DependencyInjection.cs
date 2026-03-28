using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Contract.InStock.Part;
using PuzKit3D.Contract.SupportTicket.SupportTickets;
using PuzKit3D.Contract.User;
using PuzKit3D.Modules.Partner.Infrastructure.IntegrationEventHandlers.Users;
using PuzKit3D.SharedKernel.Application.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
