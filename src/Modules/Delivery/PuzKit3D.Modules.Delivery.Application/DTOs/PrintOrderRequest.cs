using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PuzKit3D.Modules.Delivery.Application.DTOs;

public class GeneratePrintTokenRequest
{
    [JsonPropertyName("orderCodes")]
    [Required]
    [MinLength(1)]
    public List<string> OrderCodes { get; set; } = new();
}

public class PrintOrderRequest
{
    [JsonPropertyName("orderCodes")]
    [Required]
    [MinLength(1)]
    public List<string> OrderCodes { get; set; } = new();
}
