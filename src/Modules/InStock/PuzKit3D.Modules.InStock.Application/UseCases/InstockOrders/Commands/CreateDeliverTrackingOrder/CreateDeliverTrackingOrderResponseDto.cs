namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Commands.CreateDeliverTrackingOrder;

public sealed record CreateDeliverTrackingOrderResponseDto(
    string DeliveryOrderCode,
    DateTime ExpectedDeliveryTime);
