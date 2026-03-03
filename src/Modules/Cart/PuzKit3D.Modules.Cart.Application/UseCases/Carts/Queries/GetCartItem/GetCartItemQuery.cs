using PuzKit3D.Modules.Cart.Application.UseCases.Carts.Queries.GetUserCart;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Queries.GetCartItem;

public sealed record GetCartItemQuery(
    Guid UserId,
    string CartType,
    Guid ItemId) : IQuery<CartItemDto>;
