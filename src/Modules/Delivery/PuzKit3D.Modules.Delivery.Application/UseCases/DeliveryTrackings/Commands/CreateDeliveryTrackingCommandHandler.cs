using PuzKit3D.Modules.Delivery.Application.Repositories;
using PuzKit3D.Modules.Delivery.Application.Services;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Delivery.Application.UseCases.DeliveryTrackings.Commands;

public sealed class CreateDeliveryTrackingCommandHandler : ICommandTHandler<CreateDeliveryTrackingCommand, Guid>
{
    private readonly IDeliveryTrackingRepository _deliveryTrackingRepository;
    private readonly IDeliveryService _deliveryService;

    public CreateDeliveryTrackingCommandHandler(
        IDeliveryTrackingRepository deliveryTrackingRepository,
        IDeliveryService deliveryService)
    {
        _deliveryTrackingRepository = deliveryTrackingRepository;
        _deliveryService = deliveryService;
    }

    public Task<ResultT<Guid>> Handle(CreateDeliveryTrackingCommand request, CancellationToken cancellationToken)
    {
        // 1. Check OrderId tồn tại

        // 2. Check DeliveryTracking đã tồn tại cho OrderId này chưa:
        // Nếu chưa tạo DeliveryTracking đầu tiên với Type = Original, nếu có rồi thì tạo với Type = Support
        // Khi tạo phải gọi DeliveryService để tạo :
        /*
         var shippingRequest = new Delivery.Application.DTOs.CreateShippingOrderRequest
        {
            ToName = order.CustomerName,
            ToPhone = order.CustomerPhone,
            ToAddress = order.DetailAddress,
            ToWardName = order.CustomerWardName,
            ToDistrictName = order.CustomerDistrictName,
            ToProvinceName = order.CustomerProvinceName,
            OrderCode = order.Id.Value.ToString(),
            RequiredNote = "CHOXEMHANGKHONGTHU",
            Note = $"Order {order.Code}",
            Items = items,
            Content = "Puzzle 3D Product",
            CodAmount =  string.Equals(order.PaymentMethod, "COD", StringComparison.OrdinalIgnoreCase) ? (int)order.SubTotalAmount : 0
        };
        // Thông tin trả về lưu và DeliveryTracking ( DeliveryOrderCode, ExpectedDeliveryDate )
         */
        // 3. Tạo các DeliveryTrackingDetail dựa vào các OrderDetail của OrderId

        // 4. Persist vào database
    }
}
