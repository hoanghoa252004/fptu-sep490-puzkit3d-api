using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Commands.ActivatePartner;

public sealed record ActivatePartnerCommand(Guid Id) : ICommand;
