using PuzKit3D.Modules.Cart.Application.SharedResponseDto;
using PuzKit3D.SharedKernel.Application.Message.Query;

namespace PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Queries.GetCartItem;

public sealed record GetPartnerCartItemQuery(
    Guid UserId,
    Guid ItemId) : IQuery<CartItemDto>;
