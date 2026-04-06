using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Commands.CreatePartnerProductQuotation;

public sealed record CreatePartnerProductQuotationCommand(
    Guid PartnerProductRequestId,
    Guid PartnerId,
    List<CreatePartnerProductQuotationItemDto>? Items = null) : ICommandT<Guid>;

public sealed record CreatePartnerProductQuotationItemDto(
    Guid PartnerProductId,
    decimal? CustomUnitPrice = null);
