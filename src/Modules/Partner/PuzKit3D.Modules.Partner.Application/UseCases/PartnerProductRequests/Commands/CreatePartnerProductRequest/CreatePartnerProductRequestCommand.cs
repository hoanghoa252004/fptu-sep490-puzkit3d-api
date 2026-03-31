using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Commands.CreatePartnerProductRequest;

public sealed record CreatePartnerProductRequestCommand(
    Guid CustomerId,
    Guid PartnerId,
    DateTime DesiredDeliveryDate,
    List<CreatePartnerProductRequestItemRequestDto> Items) : ICommandT<Guid>;

public sealed record CreatePartnerProductRequestItemRequestDto(
    Guid PartnerProductId,
    int Quantity);
