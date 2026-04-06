using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Commands.UpdateRequirement;

public sealed record UpdateCustomDesignRequirementCommand(
    Guid Id,
    Guid? TopicId = null,
    Guid? MaterialId = null,
    Guid? AssemblyMethodId = null,
    string? Difficulty = null,
    int? MinPartQuantity = null,
    int? MaxPartQuantity = null,
    bool? IsActive = null,
    IEnumerable<Guid>? CapabilityIds = null) : ICommand;
