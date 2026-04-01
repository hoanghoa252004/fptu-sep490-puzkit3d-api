using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Commands.UpdatePartnerProductQuotationStatus;

public sealed record UpdatePartnerProductQuotationStatusCommand(
    Guid QuotationId,
    PartnerProductQuotationStatus NewStatus,
    string? Note = null) : ICommandT<Guid>;
