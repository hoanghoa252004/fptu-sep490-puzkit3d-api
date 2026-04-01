using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Queries.GetPartnerProductQuotationById;

public sealed record GetPartnerProductQuotationByIdQuery(Guid Id) : IQuery<object>;
