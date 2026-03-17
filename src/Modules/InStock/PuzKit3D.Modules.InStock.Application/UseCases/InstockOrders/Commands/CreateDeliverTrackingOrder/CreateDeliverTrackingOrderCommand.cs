using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Commands.CreateDeliverTrackingOrder;

public sealed record CreateDeliverTrackingOrderCommand(Guid OrderId) : IQuery<CreateDeliverTrackingOrderResponseDto>;
