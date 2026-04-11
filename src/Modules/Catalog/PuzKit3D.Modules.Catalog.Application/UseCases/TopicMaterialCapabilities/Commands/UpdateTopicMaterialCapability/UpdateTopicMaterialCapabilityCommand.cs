using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Commands.UpdateTopicMaterialCapability;

public sealed record UpdateTopicMaterialCapabilityCommand(
    Guid CapabilityId,
    Guid TopicMaterialCapabilityId,
    bool IsActive) : ICommandT<Guid>;
