using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Queries.GetCapabilityBySlug;

public sealed record GetCapabilityBySlugQuery(string Slug) : IQuery<object>;
