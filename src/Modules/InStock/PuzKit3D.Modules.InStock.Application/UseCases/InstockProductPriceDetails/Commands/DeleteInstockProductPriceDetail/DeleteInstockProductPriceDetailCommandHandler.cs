using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Commands.DeleteInstockProductPriceDetail;

internal sealed class DeleteInstockProductPriceDetailCommandHandler : ICommandHandler<DeleteInstockProductPriceDetailCommand>
{
    private readonly IInstockProductPriceDetailRepository _priceDetailRepository;
    private readonly IInstockOrderRepository _orderRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public DeleteInstockProductPriceDetailCommandHandler(
        IInstockProductPriceDetailRepository priceDetailRepository,
        IInstockOrderRepository orderRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _priceDetailRepository = priceDetailRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteInstockProductPriceDetailCommand request, CancellationToken cancellationToken)
    {
        var priceDetailId = InstockProductPriceDetailId.From(request.PriceDetailId);
        var priceDetail = await _priceDetailRepository.GetByIdAsync(priceDetailId, cancellationToken);

        if (priceDetail is null)
        {
            return Result.Failure(InstockProductPriceDetailError.NotFound(request.PriceDetailId));
        }

        // Check if any order details use this price detail
        var hasOrderDetails = await _orderRepository.HasOrderDetailWithPriceDetailIdAsync(
            request.PriceDetailId,
            cancellationToken);

        if (hasOrderDetails)
        {
            return Result.Failure(InstockProductPriceDetailError.CannotDeleteWithOrders(request.PriceDetailId));
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            priceDetail.Delete();
            _priceDetailRepository.Delete(priceDetail);

            return Result.Success();
        }, cancellationToken);
    }
}

