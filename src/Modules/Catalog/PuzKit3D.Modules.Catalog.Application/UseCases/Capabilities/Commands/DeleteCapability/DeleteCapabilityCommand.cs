using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Commands.DeleteCapability;

public sealed record DeleteCapabilityCommand(Guid Id) : ICommand;
