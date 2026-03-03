using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Queries.GetUserCart;

internal sealed class GetUserCartQueryHandler : IQueryHandler<GetUserCartQuery, CartDto>
{
    private readonly ICartRepository _cartRepository;

    public GetUserCartQueryHandler(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public async Task<ResultT<CartDto>> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
            request.UserId,
            request.CartType.ToUpper(),
            cancellationToken);

        if (cart == null)
            return Result.Failure<CartDto>(CartError.CartNotFound());

        var cartDto = new CartDto(
            cart.Id.Value,
            cart.UserId,
            cart.CartType,
            cart.TotalItem,
            cart.Items.Select(i => new CartItemDto(
                i.Id.Value,
                i.ItemId,
                i.UnitPrice?.Amount,
                i.InStockProductPriceDetailId,
                i.Quantity,
                i.TotalPrice?.Amount)).ToList());

        return Result.Success(cartDto);
    }
}
