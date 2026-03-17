using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PuzKit3D.Modules.Delivery.Application.DTOs;

public class GetShippingOrderDetailRequest
{
    [JsonPropertyName("orderCode")]
    [Required]
    [MaxLength(20)]
    public string OrderCode { get; set; } = string.Empty;
}
