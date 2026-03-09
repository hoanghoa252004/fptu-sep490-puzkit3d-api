using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Queries.GetInstockProductById;

public sealed record GetInstockProductByIdQuery(Guid Id) : IQuery<GetInstockProductByIdResponseDto>;
