using PuzKit3D.Modules.Cart.Application.Repositories;
using PuzKit3D.Modules.Cart.Application.UnitOfWork;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Cart.Application.UseCases.PartnerCarts.Commands.UpdateItem;

internal sealed class UpdatePartnerCartItemCommandHandler : ICommandHandler<UpdatePartnerCartItemCommand>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICartQueryRepository _queryRepository;
    private readonly ICartUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public UpdatePartnerCartItemCommandHandler(
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

    public async Task<Result> Handle(UpdatePartnerCartItemCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            if (!_currentUser.IsInRole("CUSTOMER"))
                return Result.Failure(CartError.UnauthorizedAccess());

            if (!Guid.TryParse(_currentUser.UserId, out Guid customerId))
                return Result.Failure(CartError.InvalidUserId());

            var partnerProduct = await _queryRepository.GetPartnerProductByIdAsync(request.ItemId, cancellationToken);
            if (partnerProduct == null)
                return Result.Failure(CartError.ItemNotFound());

            var cart = await _cartRepository.GetByUserIdAndCartTypeAsync(customerId, "PARTNER", cancellationToken);
            if (cart == null)
                return Result.Failure(CartError.CartNotFound());

            var updateResult = cart.UpdateItemQuantity(request.ItemId, request.Quantity);
            if (updateResult.IsFailure)
                return Result.Failure(updateResult.Error);

            _cartRepository.Update(cart);

            return Result.Success();
        }, cancellationToken);
    }
}
