using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Commands.UpdatePartner;

public sealed record UpdatePartnerCommand(
    Guid Id,
    Guid ImportServiceConfigId,
    string Name,
    string ContactEmail,
    string ContactPhone,
    string Address,
    string Slug,
    string? Description = null) : ICommand;
