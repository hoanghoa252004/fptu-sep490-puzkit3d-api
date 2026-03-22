using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Queries.GetPartnerById;

public sealed record GetPartnerByIdQuery(Guid Id) : IQuery<object>;
