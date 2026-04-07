using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetPartnerProductRequestDetail;

public sealed record GetPartnerProductRequestDetailQuery(Guid Id) : IQuery<object>;
