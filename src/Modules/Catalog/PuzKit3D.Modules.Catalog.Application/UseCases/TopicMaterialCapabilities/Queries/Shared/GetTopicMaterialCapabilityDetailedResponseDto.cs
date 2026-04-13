using PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Queries.Shared;
using PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.Shared;
using PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Queries.Shared;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Queries.Shared;

public sealed record GetTopicMaterialCapabilityDetailedResponseDto(
    Guid Id,
    GetTopicDetailedResponseDto Topic,
    GetMaterialDetailedResponseDto Material,
    GetCapabilityDetailedResponseDto Capability,
    bool IsActive);
