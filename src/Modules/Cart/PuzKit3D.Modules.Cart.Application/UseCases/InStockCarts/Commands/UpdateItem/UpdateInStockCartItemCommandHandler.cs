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

            // Get userId from JWT
            if (!Guid.TryParse(_currentUser.UserId, out Guid customerId))
            {
                return Result.Failure(CartError.InvalidUserId());
            }

            // If both quantity and price detail are null, return error
            if (!request.Quantity.HasValue && !request.InStockProductPriceDetailId.HasValue)
            {
                return Result.Failure(CartError.InvalidQuantity());
            }

            // Validate item exists
            var instockVariant = await _queryRepository.GetInStockProductVariantByIdAsync(request.ItemId, cancellationToken);

            if (instockVariant == null)
            {
                return Result.Failure(CartError.ItemNotFound());
            }

            // Check if variant is still active
            if (!instockVariant.IsActive)
            {
                return Result.Failure(CartError.ItemNotActive());
            }

            // Check inventory if quantity is provided
            if (request.Quantity.HasValue)
            {
                var inventory = await _queryRepository.GetInStockInventoryByVariantIdAsync(request.ItemId, cancellationToken);

                if (inventory == null || inventory.TotalQuantity < request.Quantity.Value)
                {
                    return Result.Failure(CartError.InsufficientStock(inventory?.TotalQuantity ?? 0));
                }
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

            // Get existing cart item to check price detail
            var existingCartItem = cart.Items.FirstOrDefault(i => i.ItemId == request.ItemId);
            if (existingCartItem == null)
            {
                return Result.Failure(CartError.CartItemNotFound());
            }

            // Determine which price detail to use
            var priceDetailIdToUse = request.InStockProductPriceDetailId ?? existingCartItem.InStockProductPriceDetailId;

            // If price detail is provided, validate it
            if (priceDetailIdToUse.HasValue)
            {
                var priceDetail = await _queryRepository.GetInStockPriceDetailByIdAsync(
                    priceDetailIdToUse.Value,
                    cancellationToken);

                if (priceDetail == null)
                {
                    return Result.Failure(CartError.PriceDetailNotFound());
                }

                if (!priceDetail.InStockProductVariantId.Equals(instockVariant.Id))
                {
                    return Result.Failure(CartError.InvalidPriceOfVariant());
                }

                if (!priceDetail.IsActive)
                {
                    return Result.Failure(CartError.PriceDetailNotActive());
                }

                // Check if price detail is the highest priority active price for this variant
                var activePriceDetails = await _queryRepository.GetAllActivePriceDetailsByVariantIdAsync(
                    request.ItemId,
                    cancellationToken);

                if (!activePriceDetails.Any())
                {
                    return Result.Failure(CartError.PriceDetailNotActive());
                }

                var highestPriorityDetail = activePriceDetails.First();
                if (priceDetail.Id != highestPriorityDetail.Id)
                {
                    return Result.Failure(CartError.PriceNotHighestPriority());
                }

                // If price detail is different from existing, update it
                if (request.InStockProductPriceDetailId.HasValue && 
                    existingCartItem.InStockProductPriceDetailId != request.InStockProductPriceDetailId.Value)
                {
                    var updatePriceResult = cart.UpdateItemPrice(request.ItemId, request.InStockProductPriceDetailId.Value);
                    if (updatePriceResult.IsFailure)
                        return Result.Failure(updatePriceResult.Error);
                }
            }

            // Update item quantity if provided
            if (request.Quantity.HasValue)
            {
                var updateResult = cart.UpdateItemQuantity(request.ItemId, request.Quantity.Value);

                if (updateResult.IsFailure)
                    return Result.Failure(updateResult.Error);
            }

            _cartRepository.Update(cart);

            return Result.Success();
        }, cancellationToken);
    }
}
