using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Queries.GetInstockProductBySlug;

public sealed record GetInstockProductBySlugQuery(string Slug) : IQuery<GetInstockProductBySlugResponseDto>;
