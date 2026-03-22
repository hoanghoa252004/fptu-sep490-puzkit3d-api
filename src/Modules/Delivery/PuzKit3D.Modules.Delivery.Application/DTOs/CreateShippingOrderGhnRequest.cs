using System.Text.Json.Serialization;

namespace PuzKit3D.Modules.Delivery.Application.DTOs;

/// <summary>
/// DTO ?? g?i t?i GHN API - tuân th? ch?t ch? format c?a GHN
/// </summary>
public class CreateShippingOrderGhnRequest
{
    [JsonPropertyName("payment_type_id")]
    public int PaymentTypeId { get; set; }

    [JsonPropertyName("note")]
    public string? Note { get; set; }

    [JsonPropertyName("required_note")]
    public string RequiredNote { get; set; } = string.Empty;

    [JsonPropertyName("from_name")]
    public string FromName { get; set; } = string.Empty;

    [JsonPropertyName("from_phone")]
    public string FromPhone { get; set; } = string.Empty;

    [JsonPropertyName("from_address")]
    public string FromAddress { get; set; } = string.Empty;

    [JsonPropertyName("from_ward_name")]
    public string FromWardName { get; set; } = string.Empty;

    [JsonPropertyName("from_district_name")]
    public string FromDistrictName { get; set; } = string.Empty;

    [JsonPropertyName("from_province_name")]
    public string FromProvinceName { get; set; } = string.Empty;

    [JsonPropertyName("client_order_code")]
    public string? ClientOrderCode { get; set; }

    [JsonPropertyName("to_name")]
    public string ToName { get; set; } = string.Empty;

    [JsonPropertyName("to_phone")]
    public string ToPhone { get; set; } = string.Empty;

    [JsonPropertyName("to_address")]
    public string ToAddress { get; set; } = string.Empty;

    [JsonPropertyName("to_ward_name")]
    public string ToWardName { get; set; } = string.Empty;

    [JsonPropertyName("to_district_name")]
    public string ToDistrictName { get; set; } = string.Empty;

    [JsonPropertyName("to_province_name")]
    public string ToProvinceName { get; set; } = string.Empty;

    [JsonPropertyName("service_type_id")]
    public int ServiceTypeId { get; set; }

    [JsonPropertyName("cod_amount")]
    public int? CodAmount { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }

    [JsonPropertyName("weight")]
    public int Weight { get; set; }

    [JsonPropertyName("pick_shift")]
    public List<int>? PickShift { get; set; }

    [JsonPropertyName("items")]
    public List<ShippingOrderItemGhn> Items { get; set; } = new();
}

public class ShippingOrderItemGhn
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("price")]
    public int? Price { get; set; }
}
