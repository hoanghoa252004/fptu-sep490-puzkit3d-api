using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Commands.UpdateInstockProductPriceDetail;

internal sealed class UpdateInstockProductPriceDetailCommandHandler : ICommandHandler<UpdateInstockProductPriceDetailCommand>
{
    private readonly IInstockProductPriceDetailRepository _priceDetailRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public UpdateInstockProductPriceDetailCommandHandler(
        IInstockProductPriceDetailRepository priceDetailRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _priceDetailRepository = priceDetailRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateInstockProductPriceDetailCommand request, CancellationToken cancellationToken)
    {
        var priceDetailId = InstockProductPriceDetailId.From(request.PriceDetailId);
        var priceDetail = await _priceDetailRepository.GetByIdAsync(priceDetailId, cancellationToken);

        if (priceDetail is null)
        {
            return Result.Failure(InstockProductPriceDetailError.NotFound(request.PriceDetailId));
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            var updateResult = priceDetail.PartialUpdate(request.UnitPrice);

            if (updateResult.IsFailure)
            {
                return Result.Failure(updateResult.Error);
            }

            _priceDetailRepository.Update(priceDetail);

            return Result.Success();
        }, cancellationToken);
    }
}
