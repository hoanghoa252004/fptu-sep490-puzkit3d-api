using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Queries.GetTopicById;

public sealed record GetTopicByIdQuery(Guid Id) : IQuery<object>;
