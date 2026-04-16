using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Delivery.Application;

public class DeliveryApplicationSettings
{
    public const string ConfigurationSection = "DeliverySettings";

    public GhnApiSettings GhnApiKey { get; set; } = new();
    public MyShopSettings MyShop { get; set; } = new();
}

public sealed class GhnApiSettings
{
    public string ApiEndpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}

public sealed class MyShopSettings
{
    public string ShopId { get; set; } = string.Empty;
    public string? DistrictId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
}
