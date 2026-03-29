using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Commands.AddItem;

internal sealed class AddItemToPartnerCartCommandHandler : ICommandHandler<AddItemToPartnerCartCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartQueryRepository _queryRepository;
    private readonly ICartUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public AddItemToPartnerCartCommandHandler(
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

    public async Task<Result> Handle(AddItemToPartnerCartCommand request, CancellationToken cancellationToken)
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


            // Validate Partner product
            var partnerProduct = await _queryRepository.GetPartnerProductByIdAsync(request.ItemId, cancellationToken);

            if (partnerProduct == null)
            {
                return Result.Failure(CartError.ItemNotFound());
            }

            if (!partnerProduct.IsActive)
            {
                return Result.Failure(CartError.ItemNotActive());
            }

            // Get or create cart
            var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
                customerId,
                "PARTNER",
                cancellationToken);

            bool isNewCart = false;
            if (cart == null)
            {
                var createResult = Domain.Entities.Carts.Cart.Create(customerId, "PARTNER");
                
                if (createResult.IsFailure)
                    return Result.Failure(createResult.Error);

                cart = createResult.Value;
                isNewCart = true;
                _cartRepository.Add(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ItemId == request.ItemId);
            if (existingItem != null)
            {
                var newQuantity = existingItem.Quantity + quantity;
                existingItem.UpdateQuantity(newQuantity);
            }
            else
            {
                // Add item to cart (no price detail for partner products)
                var addItemResult = cart.AddItem(
                    request.ItemId,
                    null,
                    quantity);

                if (addItemResult.IsFailure)
                    return Result.Failure(addItemResult.Error);
            }

            if (!isNewCart)
            {
                _cartRepository.Update(cart);
            }

            return Result.Success();
        }, cancellationToken);
    }
}
