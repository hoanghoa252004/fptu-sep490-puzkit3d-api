using PuzKit3D.Modules.Cart.Application.SharedResponseDto;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Queries.GetCart;

public sealed record GetInStockCartQuery(Guid UserId) : IQuery<CartDto>;
