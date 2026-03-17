using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Commands.UpdateInstockOrderStatus;

public sealed record UpdateInstockOrderStatusCommand(
    Guid OrderId,
    InstockOrderStatus NewStatus) : ICommand;
