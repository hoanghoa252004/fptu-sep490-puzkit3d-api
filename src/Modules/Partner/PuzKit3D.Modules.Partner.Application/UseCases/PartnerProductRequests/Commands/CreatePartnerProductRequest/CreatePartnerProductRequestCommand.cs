using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Commands.CreatePartnerProductRequest;

public sealed record CreatePartnerProductRequestCommand(
    Guid CustomerId,
    Guid PartnerId,
    DateTime DesiredDeliveryDate,
    List<CreatePartnerProductRequestItemDto> Items,
    string? Note = null) : ICommandT<Guid>;

public sealed record CreatePartnerProductRequestItemDto(
    Guid PartnerProductId,
    int Quantity);
