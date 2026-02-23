using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.InStock.Infrastructure;

public static class InStockModule
{
    public static IServiceCollection AddInStockModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddDomainEventHandlers();
        //services.AddIntegrationEventHandlers();

        //services.AddInfrastructure(configuration);

        //services.AddEndpointsFromAssembly(typeof(InStockModule).Assembly);

        return services;
    }
}
