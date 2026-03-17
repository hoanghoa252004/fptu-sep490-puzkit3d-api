using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Queries.GetGhnOrderCodeByInstockOrderId;

public sealed record GetGhnOrderCodeQuery(Guid OrderId) : IQuery<GetGhnOrderCodeResponseDto>;
