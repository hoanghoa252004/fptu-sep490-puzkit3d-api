using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Queries.GetPartnerProductById;

public sealed record GetPartnerProductByIdQuery(Guid Id) : IQuery<object>;
