using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UserCases.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResponseDto>;
