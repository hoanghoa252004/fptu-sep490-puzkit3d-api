using PuzKit3D.Modules.Delivery.Application.DTOs;

namespace PuzKit3D.Modules.Delivery.Application.Mappers;

public static class ShippingOrderMapper
{
    public static CreateShippingOrderGhnRequest ToGhnRequest(this CreateShippingOrderRequest clientRequest)
    {
        return new CreateShippingOrderGhnRequest
        {
            PaymentTypeId = clientRequest.PaymentTypeId,
            Note = clientRequest.Note,
            RequiredNote = clientRequest.RequiredNote,
            FromName = clientRequest.FromName,
            FromPhone = clientRequest.FromPhone,
            FromAddress = clientRequest.FromAddress,
            FromWardName = clientRequest.FromWardName,
            FromDistrictName = clientRequest.FromDistrictName,
            FromProvinceName = clientRequest.FromProvinceName,
            ClientOrderCode = clientRequest.ClientOrderCode,
            ToName = clientRequest.ToName,
            ToPhone = clientRequest.ToPhone,
            ToAddress = clientRequest.ToAddress,
            ToWardName = clientRequest.ToWardName,
            ToDistrictName = clientRequest.ToDistrictName,
            ToProvinceName = clientRequest.ToProvinceName,
            ServiceTypeId = clientRequest.ServiceTypeId,
            CodAmount = clientRequest.CodAmount,
            Content = clientRequest.Content,
            Weight = clientRequest.Weight,
            PickShift = clientRequest.PickShift,
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
