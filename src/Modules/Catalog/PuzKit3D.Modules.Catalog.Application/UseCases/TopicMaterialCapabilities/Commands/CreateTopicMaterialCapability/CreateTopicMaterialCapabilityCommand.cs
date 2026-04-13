using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Commands.CreateTopicMaterialCapability;

public sealed record CreateTopicMaterialCapabilityCommand(
    Guid CapabilityId,
    Guid TopicId,
    Guid MaterialId,
    bool IsActive = false) : ICommandT<Guid>;

