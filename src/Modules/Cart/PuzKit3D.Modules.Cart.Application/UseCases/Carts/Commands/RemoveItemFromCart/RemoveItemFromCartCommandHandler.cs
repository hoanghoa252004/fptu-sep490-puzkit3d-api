using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.RemoveItemFromCart;

internal sealed class RemoveItemFromCartCommandHandler : ICommandHandler<RemoveItemFromCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartUnitOfWork _unitOfWork;

    public RemoveItemFromCartCommandHandler(
        ICartRepository cartRepository,
        ICartUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var cartTypeId = CartTypeId.From(request.CartTypeId);
            
            var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
                request.UserId,
                cartTypeId,
                cancellationToken);

            if (cart == null)
                return Result.Failure(CartError.CartNotFound());

            var removeResult = cart.RemoveItem(request.ItemId);

            if (removeResult.IsFailure)
                return Result.Failure(removeResult.Error);

            _cartRepository.Update(cart);

            return Result.Success();
        }, cancellationToken);
    }
}
