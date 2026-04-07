using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetPartnerProductRequestById;

public sealed record GetPartnerProductRequestByIdQuery(Guid Id) : IQuery<object>;
