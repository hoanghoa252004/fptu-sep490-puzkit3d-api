using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Queries.GetTopicMaterialCapabilitiesByCapabilityId;

public sealed record GetTopicMaterialCapabilitiesByCapabilityIdQuery(Guid CapabilityId) : IQuery<object>;
