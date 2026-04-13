using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Commands.DeleteTopicMaterialCapability;

public sealed record DeleteTopicMaterialCapabilityCommand(
    Guid CapabilityId,
    Guid TopicMaterialCapabilityId) : ICommand;
