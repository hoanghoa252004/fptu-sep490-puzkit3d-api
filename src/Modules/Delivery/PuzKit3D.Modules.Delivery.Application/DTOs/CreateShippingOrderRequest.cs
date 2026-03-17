using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PuzKit3D.Modules.Delivery.Application.DTOs;

/// <summary>
/// DTO nhận từ Client - camelCase, kiểu dữ liệu đơn giản
/// </summary>
public class CreateShippingOrderRequest
{
    [JsonPropertyName("paymentTypeId")]
    [Range(1, 2, ErrorMessage = "paymentTypeId must be 1 (Shop) or 2 (Buyer)")]
    public int PaymentTypeId { get; set; }

    [JsonPropertyName("note")]
    [MaxLength(5000)]
    public string? Note { get; set; }

    [JsonPropertyName("requiredNote")]
    [Required]
    [RegularExpression(@"^(CHOTHUHANG|CHOXEMHANGKHONGTHU|KHONGCHOXEMHANG)$", 
        ErrorMessage = "requiredNote must be CHOTHUHANG, CHOXEMHANGKHONGTHU, or KHONGCHOXEMHANG")]
    public string RequiredNote { get; set; } = string.Empty;

    [JsonPropertyName("fromName")]
    [Required]
    [MaxLength(1024)]
    public string FromName { get; set; } = string.Empty;

    [JsonPropertyName("fromPhone")]
    [Required]
    [MaxLength(20)]
    public string FromPhone { get; set; } = string.Empty;

    [JsonPropertyName("fromAddress")]
    [Required]
    [MaxLength(1024)]
    public string FromAddress { get; set; } = string.Empty;

    [JsonPropertyName("fromWardName")]
    [Required]
    public string FromWardName { get; set; } = string.Empty;

    [JsonPropertyName("fromDistrictName")]
    [Required]
    public string FromDistrictName { get; set; } = string.Empty;

    [JsonPropertyName("fromProvinceName")]
    [Required]
    public string FromProvinceName { get; set; } = string.Empty;

    [JsonPropertyName("clientOrderCode")]
    [MaxLength(50)]
    public string? ClientOrderCode { get; set; }

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

    [JsonPropertyName("serviceTypeId")]
    [Required]
    [Range(2, 5, ErrorMessage = "serviceTypeId must be 2 (E-commerce) or 5 (Traditional)")]
    public int ServiceTypeId { get; set; }

    [JsonPropertyName("codAmount")]
    [Range(0, 50_000_000)]
    public int? CodAmount { get; set; }

    [JsonPropertyName("content")]
    [MaxLength(2000)]
    public string? Content { get; set; }

    [JsonPropertyName("weight")]
    [Required]
    [Range(1, 50_000)]
    public int Weight { get; set; }

    [JsonPropertyName("pickShift")]
    public List<int>? PickShift { get; set; }

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
