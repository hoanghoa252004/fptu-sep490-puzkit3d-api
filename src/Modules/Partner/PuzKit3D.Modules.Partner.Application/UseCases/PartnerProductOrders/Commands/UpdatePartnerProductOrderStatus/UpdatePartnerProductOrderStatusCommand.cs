using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;
using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Commands.UpdatePartnerProductOrderStatus;

public sealed record UpdatePartnerProductOrderStatusCommand(
    Guid OrderId,
    string NewStatus) : ICommandT<Guid>;