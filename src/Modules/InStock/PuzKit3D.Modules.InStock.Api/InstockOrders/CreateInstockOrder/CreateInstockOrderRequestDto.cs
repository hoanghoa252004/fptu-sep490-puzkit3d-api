namespace PuzKit3D.Modules.InStock.Api.InstockOrders.CreateInstockOrder;

public sealed record CreateInstockOrderRequestDto(
    string CustomerName,
    string CustomerPhone,
    string CustomerEmail,
    string CustomerProvinceCode,
    string CustomerProvinceName,
    string CustomerDistrictCode,
    string CustomerDistrictName,
    string CustomerWardCode,
    string CustomerWardName,
    List<CartItemRequestDto> CartItems,
    decimal SubTotalAmount,
    decimal ShippingFee,
    int UsedCoinAmount,
    decimal GrandTotalAmount,
    string PaymentMethod);

public sealed record CartItemRequestDto(
    Guid ItemId,
    Guid PriceDetailId,
    int Quantity);
