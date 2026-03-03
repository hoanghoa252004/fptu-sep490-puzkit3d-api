using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.AddItemToCart;

internal sealed class AddItemToCartCommandHandler : ICommandHandler<AddItemToCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartUnitOfWork _unitOfWork;

    public AddItemToCartCommandHandler(
        ICartRepository cartRepository,
        ICartUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var cartTypeId = CartTypeId.From(request.CartTypeId);
            
            var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
                request.UserId,
                cartTypeId,
                cancellationToken);

            if (cart == null)
            {
                var createResult = Domain.Entities.Carts.Cart.Create(request.UserId, cartTypeId);
                
                if (createResult.IsFailure)
                    return Result.Failure(createResult.Error);

                cart = createResult.Value;
                _cartRepository.Add(cart);
            }

            var addItemResult = cart.AddItem(
                request.ItemId,
                request.UnitPrice,
                request.InStockProductPriceDetailId,
                request.Quantity);

            if (addItemResult.IsFailure)
                return Result.Failure(addItemResult.Error);

            _cartRepository.Update(cart);

            return Result.Success();
        }, cancellationToken);
    }
}
