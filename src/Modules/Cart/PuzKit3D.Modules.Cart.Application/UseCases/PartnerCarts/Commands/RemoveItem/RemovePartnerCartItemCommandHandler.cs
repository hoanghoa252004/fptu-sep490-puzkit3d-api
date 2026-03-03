using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Commands.RemoveItem;

internal sealed class RemovePartnerCartItemCommandHandler : ICommandHandler<RemovePartnerCartItemCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public RemovePartnerCartItemCommandHandler(
        ICartRepository cartRepository,
        ICartUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(RemovePartnerCartItemCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            if (!_currentUser.IsInRole("CUSTOMER"))
                return Result.Failure(CartError.UnauthorizedAccess());

            if (!Guid.TryParse(_currentUser.UserId, out Guid customerId))
                return Result.Failure(CartError.InvalidUserId());

            var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(customerId, "PARTNER", cancellationToken);

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
