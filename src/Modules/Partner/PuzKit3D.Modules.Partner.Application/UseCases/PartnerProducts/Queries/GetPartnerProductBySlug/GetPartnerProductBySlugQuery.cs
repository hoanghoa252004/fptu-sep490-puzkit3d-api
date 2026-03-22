using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Queries.GetPartnerProductBySlug;

public sealed record GetPartnerProductBySlugQuery(string Slug) : IQuery<object>;
