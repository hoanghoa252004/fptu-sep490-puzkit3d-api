using PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Queries.Shared;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Queries.GetCapabilitiesByTopicAndMaterial;

public sealed record GetCapabilitiesByTopicAndMaterialQuery(
    Guid TopicId,
    Guid MaterialId) : IQuery<List<GetCapabilityBasicResponseDto>>;
