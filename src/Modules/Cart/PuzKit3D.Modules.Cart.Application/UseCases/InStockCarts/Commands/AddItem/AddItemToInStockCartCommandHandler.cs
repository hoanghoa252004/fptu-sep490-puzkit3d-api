using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.InStockCarts.Commands.AddItem;

internal sealed class AddItemToInStockCartCommandHandler : ICommandHandler<AddItemToInStockCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartQueryRepository _queryRepository;
    private readonly ICartUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public AddItemToInStockCartCommandHandler(
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

    public async Task<Result> Handle(AddItemToInStockCartCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Get userId from JWT
            if (!Guid.TryParse(_currentUser.UserId, out Guid customerId))
            {
                return Result.Failure(CartError.InvalidUserId());
            }

            // Set default quantity to 1 if not provided
            var quantity = request.Quantity ?? 1;

            // Validate InStock variant
            var instockVariant = await _queryRepository.GetInStockProductVariantByIdAsync(request.ItemId, cancellationToken);

            if (instockVariant == null)
            {
                return Result.Failure(CartError.ItemNotFound());
            }

            if (!instockVariant.IsActive)
            {
                return Result.Failure(CartError.ItemNotActive());
            }

            // Check inventory
            var inventory = await _queryRepository.GetInStockInventoryByVariantIdAsync(request.ItemId, cancellationToken);

            if (inventory == null || inventory.TotalQuantity < quantity)
            {
                return Result.Failure(CartError.InsufficientStock(inventory?.TotalQuantity ?? 0));
            }

            // Validate price detail
            var priceDetail = await _queryRepository.GetInStockPriceDetailByIdAsync(request.InStockProductPriceDetailId, cancellationToken);

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

            // Get or create cart
            var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
                customerId,
                "INSTOCK",
                cancellationToken);

            bool isNewCart = false;
            if (cart == null)
            {
                var createResult = Domain.Entities.Carts.Cart.Create(customerId, "INSTOCK");
                
                if (createResult.IsFailure)
                    return Result.Failure(createResult.Error);

                cart = createResult.Value;
                isNewCart = true;
                _cartRepository.Add(cart);
            }

            // Add item to cart
            var addItemResult = cart.AddItem(
                request.ItemId,
                request.InStockProductPriceDetailId,
                quantity);

            if (addItemResult.IsFailure)
                return Result.Failure(addItemResult.Error);

            if (!isNewCart)
            {
                _cartRepository.Update(cart);
            }

            return Result.Success();
        }, cancellationToken);
    }
}
