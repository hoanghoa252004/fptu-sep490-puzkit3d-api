using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Commands.CreatePartner;

public sealed record CreatePartnerCommand(
    string Name,
    string ContactEmail,
    string ContactPhone,
    string Address,
    string Slug,
    Guid ImportServiceConfigId,
    string? Description = null) : ICommandT<Guid>;
