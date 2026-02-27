using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAssemblyMethodBySlug;

public sealed record GetAssemblyMethodBySlugQuery(string Slug) : IQuery<GetAssemblyMethodBySlugPublicResponseDto>;


