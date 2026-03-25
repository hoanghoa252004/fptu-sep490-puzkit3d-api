using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Commands.DeleteImportServiceConfig;

public sealed record DeleteImportServiceConfigCommand(Guid Id) : ICommand;
