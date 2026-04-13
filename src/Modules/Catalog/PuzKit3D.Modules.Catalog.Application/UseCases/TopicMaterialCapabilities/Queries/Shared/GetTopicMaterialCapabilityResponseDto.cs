using PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Queries.Shared;
using PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.Shared;
using PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Queries.Shared;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Queries.Shared;

public sealed record GetTopicMaterialCapabilityResponseDto(
    Guid Id,
    GetTopicResponseDto Topic,
    GetMaterialResponseDto Material,
    GetCapabilityResponseDto Capability);
