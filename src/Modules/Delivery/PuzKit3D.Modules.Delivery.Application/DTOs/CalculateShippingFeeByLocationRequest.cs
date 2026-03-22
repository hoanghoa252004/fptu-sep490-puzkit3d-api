using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PuzKit3D.Modules.Delivery.Application.DTOs;

public class CalculateShippingFeeByLocationRequest
{
    [JsonPropertyName("provinceName")]
    [Required]
    [MinLength(2, ErrorMessage = "Province name must be at least 2 characters")]
    public string ProvinceName { get; set; } = string.Empty;

    [JsonPropertyName("districtName")]
    [Required]
    [MinLength(2, ErrorMessage = "District name must be at least 2 characters")]
    public string DistrictName { get; set; } = string.Empty;

    [JsonPropertyName("wardName")]
    [Required]
    [MinLength(2, ErrorMessage = "Ward name must be at least 2 characters")]
    public string WardName { get; set; } = string.Empty;
}
