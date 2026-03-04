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

            // Set default quantity to 1 if not provided
            var quantity = request.Quantity ?? 1;

            // Validate Partner product
            var partnerProduct = await _queryRepository.GetPartnerProductByIdAsync(request.ItemId, cancellationToken);

            if (partnerProduct == null)
            {
                return Result.Failure(CartError.ItemNotFound());
            }

            // Get or create cart
            var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(
                customerId,
                "PARTNER",
                cancellationToken);

            if (cart == null)
            {
                var createResult = Domain.Entities.Carts.Cart.Create(customerId, "PARTNER");
                
                if (createResult.IsFailure)
                    return Result.Failure(createResult.Error);

                cart = createResult.Value;
                _cartRepository.Add(cart);
            }

            // Add item to cart (no unitPrice for partner products in cart)
            var addItemResult = cart.AddItem(
                request.ItemId,
                null,
                null,
                quantity);

            if (addItemResult.IsFailure)
                return Result.Failure(addItemResult.Error);

            _cartRepository.Update(cart);

            return Result.Success();
        }, cancellationToken);
    }
}
