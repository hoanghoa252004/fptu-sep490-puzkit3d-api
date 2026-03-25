using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Partner.Application.UseCases.ImportServiceConfigs.Commands.ActivateImportServiceConfig;

public sealed record ActivateImportServiceConfigCommand(Guid Id) : ICommand;
