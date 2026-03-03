using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.RemoveCartItem;

internal sealed class RemoveCartItemCommandHandler : ICommandHandler<RemoveCartItemCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public RemoveCartItemCommandHandler(
        ICartRepository cartRepository,
        ICartUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Check if user is customer
            if (!_currentUser.IsInRole("CUSTOMER"))
            {
                return Result.Failure(CartError.UnauthorizedAccess());
            }

            // Get userId from JWT
            if (!Guid.TryParse(_currentUser.UserId, out Guid customerId))
            {
                return Result.Failure(CartError.InvalidUserId());
            }

            // Normalize and validate itemType
            var itemType = request.ItemType.ToUpper();
            if (itemType != "INSTOCK" && itemType != "PARTNER")
            {
                return Result.Failure(CartError.InvalidItemType());
            }

            // Get cart
            var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
                customerId,
                itemType,
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
