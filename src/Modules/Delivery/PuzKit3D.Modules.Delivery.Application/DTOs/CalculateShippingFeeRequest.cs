using System.Text.Json.Serialization;

namespace PuzKit3D.Modules.Delivery.Application.DTOs;

public class CalculateShippingFeeRequest
{
    [JsonPropertyName("to_district_id")]
    public int ToDistrictId { get; set; }

    [JsonPropertyName("to_ward_code")]
    public string ToWardCode { get; set; } = string.Empty;

    [JsonPropertyName("weight")]
    public int Weight { get; set; }

    [JsonPropertyName("service_id")]
    public int ServiceId { get; set; }

    [JsonPropertyName("service_type_id")]
    public int ServiceTypeId { get; set; }
}
