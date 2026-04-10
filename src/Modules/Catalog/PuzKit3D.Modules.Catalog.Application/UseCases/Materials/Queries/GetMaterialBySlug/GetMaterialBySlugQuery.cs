using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.GetMaterialBySlug;

public sealed record GetMaterialBySlugQuery(string Slug) : IQuery<object>;
