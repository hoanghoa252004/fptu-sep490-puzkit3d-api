using MediatR;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockOrders.Commands.UpdateInstockOrderStatus;

internal sealed class UpdateInstockOrderStatusCommandHandler : ICommandHandler<UpdateInstockOrderStatusCommand>
{
    private readonly IInstockOrderRepository _orderRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public UpdateInstockOrderStatusCommandHandler(
        IInstockOrderRepository orderRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateInstockOrderStatusCommand request, CancellationToken cancellationToken)
    {
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
