using PuzKit3D.Modules.Delivery.Application.DTOs;

namespace PuzKit3D.Modules.Delivery.Application.Mappers;

public static class ShippingOrderMapper
{
    public static CreateShippingOrderGhnRequest ToGhnRequest(this CreateShippingOrderRequest clientRequest, SenderInfo senderInfo)
    {
        return new CreateShippingOrderGhnRequest
        {
            PaymentTypeId = 2,  // Always 2 (Shop pays)
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



