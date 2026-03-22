using MediatR;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Commands.UpdateInstockOrderStatus;

internal sealed class UpdateInstockOrderStatusCommandHandler : ICommandHandler<UpdateInstockOrderStatusCommand>
{
    private readonly IInstockOrderRepository _orderRepository;
    private readonly IInStockUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public UpdateInstockOrderStatusCommandHandler(
        IInstockOrderRepository orderRepository,
        IInStockUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(UpdateInstockOrderStatusCommand request, CancellationToken cancellationToken)
    {
        // Check customer authorization - customer ch? ???c update status thành Cancelled ho?c Completed
        if (_currentUser.IsInRole(Roles.Customer))
        {
            if (request.NewStatus != InstockOrderStatus.Cancelled && 
                request.NewStatus != InstockOrderStatus.Completed)
            {
                return Result.Failure(Error.Validation(
                    "CustomerStatusRestriction",
                    "Customer can only update order status to Cancelled or Completed"));
            }
        }

        var orderId = InstockOrderId.From(request.OrderId);
        var order = await _orderRepository.GetByIdWithDetailsAsync(orderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure(InstockOrderError.NotFound(request.OrderId));
        }

        // UpdateStatus already validates transition and returns Result
        var updateResult = order.UpdateStatus(request.NewStatus);
        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            _orderRepository.Update(order);
            return Result.Success();
        }, cancellationToken);
    }
}
