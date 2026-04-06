using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Queries.GetPartnerProductOrderById;

public sealed record GetPartnerProductOrderByIdQuery(Guid Id) : IQuery<object>;
