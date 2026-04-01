using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Commands.UpdatePartnerProductRequestStatus;

public sealed record UpdatePartnerProductRequestStatusCommand(
    Guid RequestId,
    PartnerProductRequestStatus NewStatus,
    string? Note = null) : ICommandT<Guid>;
