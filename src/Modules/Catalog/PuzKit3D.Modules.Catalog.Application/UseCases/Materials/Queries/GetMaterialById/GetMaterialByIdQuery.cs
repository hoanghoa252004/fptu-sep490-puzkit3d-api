using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.GetMaterialById;

public sealed record GetMaterialByIdQuery(Guid Id) : IQuery<object>;
