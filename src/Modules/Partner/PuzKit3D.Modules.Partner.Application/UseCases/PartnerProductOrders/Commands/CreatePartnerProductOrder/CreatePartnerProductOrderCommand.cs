using PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Commands.CreatePartnerProductRequest;
using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Commands.CreatePartnerProductOrder;

public sealed record CreatePartnerProductOrderCommand(
    Guid QuotationId,
    Guid CustomerId,
    string CustomerName,
    string CustomerPhone,
    string CustomerEmail,
    string CustomerProvinceName,
    string CustomerDistrictName,
    string CustomerWardName,
    string DetailAddress,
    int UserCoinAmount,
    decimal ShippingFee,
    string PaymentMethod,
    List<PartnerProductOrderDetailsItemDto>? Items) : ICommandT<Guid>;

public sealed record PartnerProductOrderDetailsItemDto(
    Guid PartnerProductId,
    int Quantity,
    decimal UnitPrice);
