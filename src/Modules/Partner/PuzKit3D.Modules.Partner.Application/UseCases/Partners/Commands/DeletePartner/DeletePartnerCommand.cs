using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Commands.DeletePartner;

public sealed record DeletePartnerCommand(Guid Id) : ICommand;
