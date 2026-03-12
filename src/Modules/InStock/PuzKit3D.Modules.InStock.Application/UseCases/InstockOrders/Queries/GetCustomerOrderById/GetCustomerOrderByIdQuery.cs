using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Queries.GetCustomerOrderById;

public sealed record GetCustomerOrderByIdQuery(Guid OrderId) : IQuery<GetCustomerOrderByIdResponseDto>;
