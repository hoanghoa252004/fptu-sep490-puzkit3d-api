using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Commands.UpdateItem;

internal sealed class UpdateInStockCartItemCommandHandler : ICommandHandler<UpdateInStockCartItemCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartQueryRepository _queryRepository;
    private readonly ICartUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public UpdateInStockCartItemCommandHandler(
        ICartRepository cartRepository,
        ICartQueryRepository queryRepository,
        ICartUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _cartRepository = cartRepository;
        _queryRepository = queryRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(UpdateInStockCartItemCommand request, CancellationToken cancellationToken)
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

            // Validate item exists
            var instockVariant = await _queryRepository.GetInStockProductVariantByIdAsync(request.ItemId, cancellationToken);

            if (instockVariant == null)
            {
                return Result.Failure(CartError.ItemNotFound());
            }

            // Check inventory
            var inventory = await _queryRepository.GetInStockInventoryByVariantIdAsync(request.ItemId, cancellationToken);

            if (inventory == null || inventory.TotalQuantity < request.Quantity)
            {
                return Result.Failure(CartError.InsufficientStock(inventory?.TotalQuantity ?? 0));
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

            // Update item quantity
            var updateResult = cart.UpdateItemQuantity(request.ItemId, request.Quantity);

            if (updateResult.IsFailure)
                return Result.Failure(updateResult.Error);

            _cartRepository.Update(cart);

            return Result.Success();
        }, cancellationToken);
    }
}
