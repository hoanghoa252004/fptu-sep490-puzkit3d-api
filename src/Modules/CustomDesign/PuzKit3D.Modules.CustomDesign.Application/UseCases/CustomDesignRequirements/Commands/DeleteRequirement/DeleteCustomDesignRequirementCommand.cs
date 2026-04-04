using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Commands.DeleteRequirement;

public sealed record DeleteCustomDesignRequirementCommand(Guid Id) : ICommand;
