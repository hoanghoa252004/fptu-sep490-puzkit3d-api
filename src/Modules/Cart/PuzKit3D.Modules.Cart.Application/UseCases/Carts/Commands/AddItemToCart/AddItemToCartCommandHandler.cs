using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.Carts.Commands.AddItemToCart;

internal sealed class AddItemToCartCommandHandler : ICommandHandler<AddItemToCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartQueryRepository _queryRepository;
    private readonly ICartUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public AddItemToCartCommandHandler(
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

    public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
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

            // Set default quantity to 1 if not provided
            var quantity = request.Quantity ?? 1;

            // Check if item exists based on itemType
            decimal? unitPrice = null;
            Guid? inStockProductPriceDetailId = null;

            if (itemType == "PARTNER")
            {
                var partnerProduct = await _queryRepository.GetPartnerProductByIdAsync(request.ItemId, cancellationToken);

                if (partnerProduct == null)
                {
                    return Result.Failure(CartError.ItemNotFound());
                }
            }
            else // INSTOCK
            {
                var instockVariant = await _queryRepository.GetInStockProductVariantByIdAsync(request.ItemId, cancellationToken);

                if (instockVariant == null)
                {
                    return Result.Failure(CartError.ItemNotFound());
                }

                // Check inventory
                var inventory = await _queryRepository.GetInStockInventoryByVariantIdAsync(request.ItemId, cancellationToken);

                if (inventory == null || inventory.TotalQuantity < quantity)
                {
                    return Result.Failure(CartError.InsufficientStock(inventory?.TotalQuantity ?? 0));
                }

                // Get price detail for instock
                var priceDetail = await _queryRepository.GetActiveInStockPriceDetailByVariantIdAsync(request.ItemId, cancellationToken);

                if (priceDetail != null)
                {
                    inStockProductPriceDetailId = priceDetail.Id;
                    unitPrice = priceDetail.UnitPrice;
                }
            }

            // Get or create cart
            var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
                customerId,
                itemType,
                cancellationToken);

            if (cart == null)
            {
                var createResult = Domain.Entities.Carts.Cart.Create(customerId, itemType);
                
                if (createResult.IsFailure)
                    return Result.Failure(createResult.Error);

                cart = createResult.Value;
                _cartRepository.Add(cart);
            }

            // Add item to cart
            var addItemResult = cart.AddItem(
                request.ItemId,
                unitPrice,
                inStockProductPriceDetailId,
                quantity);

            if (addItemResult.IsFailure)
                return Result.Failure(addItemResult.Error);

            _cartRepository.Update(cart);

            return Result.Success();
        }, cancellationToken);
    }
}


