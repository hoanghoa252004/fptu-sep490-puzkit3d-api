using PuzKit3D.Contract.InStock.InstockOrders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders.DomainEvents;
using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders.DomainEvents;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Net.NetworkInformation;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductOrders.Commands.UpdatePartnerProductOrderStatus;

internal sealed class UpdatePartnerProductOrderStatusCommandHandler : ICommandTHandler<UpdatePartnerProductOrderStatusCommand, Guid>
{
    private readonly IPartnerProductOrderRepository _orderRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;
    private readonly IPartnerProductRepository _productRepository;

    public UpdatePartnerProductOrderStatusCommandHandler(
        IPartnerProductOrderRepository orderRepository,
        IPartnerUnitOfWork unitOfWork,
        IPartnerProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task<ResultT<Guid>> Handle(
        UpdatePartnerProductOrderStatusCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.ExecuteAsync(async () =>
        {
            // Lấy order
            var order = await _orderRepository.GetByIdWithDetailsAsync(
                PartnerProductOrderId.From(command.OrderId),
                cancellationToken);

            if (order is null)
            {
                return Result.Failure<Guid>(PartnerProductOrderError.NotFound(command.OrderId));
            }

            if (!Enum.TryParse<PartnerProductOrderStatus>(command.NewStatus, out var newStatus))
            {
                return Result.Failure<Guid>(PartnerProductOrderError.InvalidStatus(command.NewStatus));
            }

            // Validate status transition
            var currentStatus = order.Status;
            if (!PartnerProductOrderStatusTransition.IsValidTransition(currentStatus, newStatus))
            {
                return Result.Failure<Guid>(
                    PartnerProductOrderError.InvalidStatusTransition(currentStatus, newStatus));
            }

            // Update status
            var updateResult = order.UpdateStatus(newStatus);
            if (updateResult.IsFailure)
            {
                return Result.Failure<Guid>(updateResult.Error);
            }

            // Raise specific cancelled event for inventory reversal
            if ((newStatus == PartnerProductOrderStatus.CancelledByCustomer
                || newStatus == PartnerProductOrderStatus.CancelledByStaff)
                && currentStatus >= PartnerProductOrderStatus.ReceivedAtWarehouse)
            {
                foreach (var item in order.Details)
                {
                    var product = await _productRepository.GetByIdAsync(item.PartnerProductId, cancellationToken);
                    if (product is not null)
                    {
                        product.IncreaseQuantity(item.Quantity);
                    }
                }
            }

            _orderRepository.Update(order);

            return Result.Success(order.Id.Value);
        }, cancellationToken);

        return result;
    }
}
