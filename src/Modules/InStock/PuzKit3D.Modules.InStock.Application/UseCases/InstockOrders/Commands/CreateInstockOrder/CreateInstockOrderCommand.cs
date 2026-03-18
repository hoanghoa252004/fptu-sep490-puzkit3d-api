using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Commands.CreateInstockOrder;

public sealed record CreateInstockOrderCommand(
    Guid CustomerId,
    string CustomerName,
    string CustomerPhone,
    string CustomerEmail,
    string CustomerProvinceName,
    string CustomerDistrictName,
    string DetailAddress,
    string CustomerWardName,
    List<CartItemDto> CartItems,
    decimal ShippingFee,
    int UsedCoinAmount,
    decimal GrandTotalAmount,
    string PaymentMethod) : ICommandT<Guid>;
