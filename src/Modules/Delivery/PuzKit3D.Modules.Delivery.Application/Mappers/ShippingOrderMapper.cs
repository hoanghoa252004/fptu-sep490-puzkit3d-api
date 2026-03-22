using PuzKit3D.Modules.Delivery.Application.DTOs;

namespace PuzKit3D.Modules.Delivery.Application.Mappers;

public static class ShippingOrderMapper
{
    public static CreateShippingOrderGhnRequest ToGhnRequest(this CreateShippingOrderRequest clientRequest, SenderInfo senderInfo)
    {
        // Determine payment type based on COD amount
        // If CodAmount == 0: payment_type_id = 1 (Shop pays for online orders)
        // If CodAmount > 0: payment_type_id = 2 (Customer pays via COD)
        var paymentTypeId = clientRequest.CodAmount == 0 ? 1 : 2;

        return new CreateShippingOrderGhnRequest
        {
            PaymentTypeId = paymentTypeId,
            Note = clientRequest.Note,
            RequiredNote = clientRequest.RequiredNote,
            FromName = senderInfo.Name,
            FromPhone = senderInfo.Phone,
            FromAddress = senderInfo.Address,
            FromWardName = senderInfo.Ward,
            FromDistrictName = senderInfo.District,
            FromProvinceName = senderInfo.Province,
            ClientOrderCode = clientRequest.OrderCode,
            ToName = clientRequest.ToName,
            ToPhone = clientRequest.ToPhone,
            ToAddress = clientRequest.ToAddress,
            ToWardName = clientRequest.ToWardName,
            ToDistrictName = clientRequest.ToDistrictName,
            ToProvinceName = clientRequest.ToProvinceName,
            ServiceTypeId = 2,  // Always 2 (E-commerce)
            CodAmount = clientRequest.CodAmount,
            Content = clientRequest.Content,
            Weight = 1,  // Always 1
            PickShift = new List<int> { 1 },  // Always [1]
            Items = clientRequest.Items.Select(item => new ShippingOrderItemGhn
            {
                Name = item.Name,
                Code = item.Code,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList()
        };
    }
}



