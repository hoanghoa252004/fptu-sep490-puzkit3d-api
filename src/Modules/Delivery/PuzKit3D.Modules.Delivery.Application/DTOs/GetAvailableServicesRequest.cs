using System.Text.Json.Serialization;

namespace PuzKit3D.Modules.Delivery.Application.DTOs;

public class GetAvailableServicesRequest
{
    [JsonPropertyName("shop_id")]
    public int ShopId { get; set; }

    [JsonPropertyName("from_district")]
    public int FromDistrict { get; set; }

    [JsonPropertyName("to_district")]
    public int ToDistrict { get; set; }
}
