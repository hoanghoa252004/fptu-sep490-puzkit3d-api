using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Commands.CreateInstockOrder;

public sealed record CreateInstockOrderCommand(
    Guid CustomerId,
    string CustomerName,
    string CustomerPhone,
    string CustomerEmail,
    string CustomerProvinceCode,
    string CustomerProvinceName,
    string CustomerDistrictCode,
    string CustomerDistrictName,
    string CustomerWardCode,
    string CustomerWardName,
    List<CartItemDto> CartItems,
    decimal SubTotalAmount,
    decimal ShippingFee,
    int UsedCoinAmount,
    decimal GrandTotalAmount,
    string PaymentMethod) : ICommandT<Guid>;
