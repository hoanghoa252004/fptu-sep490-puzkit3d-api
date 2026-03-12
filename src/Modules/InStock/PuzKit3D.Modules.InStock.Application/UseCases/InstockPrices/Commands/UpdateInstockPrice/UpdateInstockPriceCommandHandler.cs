using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Commands.UpdateInstockPrice;

internal sealed class UpdateInstockPriceCommandHandler : ICommandHandler<UpdateInstockPriceCommand>
{
    private readonly IInstockPriceRepository _priceRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public UpdateInstockPriceCommandHandler(
        IInstockPriceRepository priceRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _priceRepository = priceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateInstockPriceCommand request, CancellationToken cancellationToken)
    {
        var priceId = InstockPriceId.From(request.PriceId);
        var price = await _priceRepository.GetByIdAsync(priceId, cancellationToken);

        if (price is null)
        {
            return Result.Failure(InstockPriceError.NotFound(request.PriceId));
        }

        if (request.Priority.HasValue)
        {
            var existingByPriority = await _priceRepository.GetByPriorityAsync(request.Priority.Value, cancellationToken);
            if (existingByPriority is not null && existingByPriority.Id != price.Id)
            {
                return Result.Failure(InstockPriceError.DuplicatePriority(request.Priority.Value));
            }
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            var updateResult = price.PartialUpdate(
                request.Name,
                request.EffectiveFrom,
                request.EffectiveTo,
                request.Priority,
                request.IsActive);

            if (updateResult.IsFailure)
            {
                return Result.Failure(updateResult.Error);
            }

            _priceRepository.Update(price);

            return Result.Success();
        }, cancellationToken);
    }
}
