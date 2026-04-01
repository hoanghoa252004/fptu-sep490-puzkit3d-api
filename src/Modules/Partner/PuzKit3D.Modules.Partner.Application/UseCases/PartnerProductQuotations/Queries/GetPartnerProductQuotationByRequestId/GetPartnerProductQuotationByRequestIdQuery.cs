using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductQuotations.Queries.GetPartnerProductQuotationByRequestId;

public sealed record GetPartnerProductQuotationByRequestIdQuery(Guid RequestId) : IQuery<object>;
