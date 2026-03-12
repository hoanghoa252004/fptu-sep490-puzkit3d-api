using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Commands.RemoveItem;

internal sealed class RemoveInStockCartItemCommandHandler : ICommandHandler<RemoveInStockCartItemCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public RemoveInStockCartItemCommandHandler(
        ICartRepository cartRepository,
        ICartUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(RemoveInStockCartItemCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {

            // Get userId from JWT
            if (!Guid.TryParse(_currentUser.UserId, out Guid customerId))
            {
                return Result.Failure(CartError.InvalidUserId());
            }

            // Get cart
            var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
                customerId,
                "INSTOCK",
                cancellationToken);

            if (cart == null)
            {
                return Result.Failure(CartError.CartNotFound());
            }

            // Remove item from cart
            var removeResult = cart.RemoveItem(request.ItemId);

            if (removeResult.IsFailure)
                return Result.Failure(removeResult.Error);

            _cartRepository.Update(cart);

            return Result.Success();
        }, cancellationToken);
    }
}
