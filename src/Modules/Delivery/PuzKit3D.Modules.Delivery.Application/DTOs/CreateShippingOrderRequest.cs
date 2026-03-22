using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PuzKit3D.Modules.Delivery.Application.DTOs;

/// <summary>
/// DTO nh?n t? Client - camelCase, ki?u d? li?u ??n gi?n
/// Sender info t? DeliverySettings.MyShopSettings (ko c?n truy?n)
/// PaymentTypeId luôn = 2 (Shop pays)
/// ServiceTypeId luôn = 2 (E-commerce)
/// Weight luôn = 1
/// PickShift luôn = [1]
/// </summary>
public class CreateShippingOrderRequest
{
    [JsonPropertyName("note")]
    [MaxLength(5000)]
    public string? Note { get; set; }

    [JsonPropertyName("requiredNote")]
    [Required]
    [RegularExpression(@"^(CHOTHUHANG|CHOXEMHANGKHONGTHU|KHONGCHOXEMHANG)$", 
        ErrorMessage = "requiredNote must be CHOTHUHANG, CHOXEMHANGKHONGTHU, or KHONGCHOXEMHANG")]
    public string RequiredNote { get; set; } = string.Empty;

    [JsonPropertyName("orderCode")]
    [MaxLength(50)]
    public string? OrderCode { get; set; }

    [JsonPropertyName("toName")]
    [Required]
    [MaxLength(1024)]
    public string ToName { get; set; } = string.Empty;

    [JsonPropertyName("toPhone")]
    [Required]
    [MaxLength(20)]
    public string ToPhone { get; set; } = string.Empty;

    [JsonPropertyName("toAddress")]
    [Required]
    [MaxLength(1024)]
    public string ToAddress { get; set; } = string.Empty;

    [JsonPropertyName("toWardName")]
    [Required]
    public string ToWardName { get; set; } = string.Empty;

    [JsonPropertyName("toDistrictName")]
    [Required]
    public string ToDistrictName { get; set; } = string.Empty;

    [JsonPropertyName("toProvinceName")]
    [Required]
    public string ToProvinceName { get; set; } = string.Empty;

    [JsonPropertyName("codAmount")]
    [Range(0, 50_000_000)]
    public int? CodAmount { get; set; }

    [JsonPropertyName("content")]
    [MaxLength(2000)]
    public string? Content { get; set; }

    [JsonPropertyName("items")]
    [Required]
    [MinLength(1)]
    public List<ShippingOrderItem> Items { get; set; } = new();
}

public class ShippingOrderItem
{
    [JsonPropertyName("name")]
    [Required]
    [MaxLength(500)]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    [MaxLength(100)]
    public string? Code { get; set; }

    [JsonPropertyName("quantity")]
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [JsonPropertyName("price")]
    [Range(0, int.MaxValue)]
    public int? Price { get; set; }
}

