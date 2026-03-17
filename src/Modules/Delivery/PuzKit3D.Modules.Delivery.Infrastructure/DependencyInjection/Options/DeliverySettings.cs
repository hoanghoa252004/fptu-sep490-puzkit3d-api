using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Delivery.Infrastructure.DependencyInjection.Options;

public sealed class DeliverySettings
{
    public const string ConfigurationSection = "DeliverySettings";
    
    public GhnApiSettings GhnApiKey { get; set; } = new();
}

public sealed class GhnApiSettings
{
    public string ApiEndpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}
