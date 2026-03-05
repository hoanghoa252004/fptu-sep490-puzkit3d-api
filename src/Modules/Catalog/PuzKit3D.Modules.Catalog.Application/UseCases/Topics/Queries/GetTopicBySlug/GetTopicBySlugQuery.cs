using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Queries.GetTopicBySlug;

public sealed record GetTopicBySlugQuery(string Slug) : IQuery<object>;
