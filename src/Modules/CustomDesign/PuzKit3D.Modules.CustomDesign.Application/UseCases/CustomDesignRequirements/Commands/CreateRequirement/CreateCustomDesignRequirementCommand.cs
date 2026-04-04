using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Commands.CreateRequirement;

public sealed record CreateCustomDesignRequirementCommand(
    Guid TopicId,
    Guid MaterialId,
    Guid AssemblyMethodId,
    string Difficulty,
    int MinPartQuantity,
    int MaxPartQuantity,
    IEnumerable<Guid> CapabilityIds) : ICommandT<Guid>;
