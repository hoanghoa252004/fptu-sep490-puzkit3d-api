using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Queries.GetPartnerBySlug;

public sealed record GetPartnerBySlugQuery(string Slug) : IQuery<object>;
