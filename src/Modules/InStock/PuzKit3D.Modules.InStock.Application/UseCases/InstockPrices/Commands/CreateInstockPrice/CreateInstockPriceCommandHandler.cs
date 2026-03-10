using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Commands.CreateInstockPrice;

internal sealed class CreateInstockPriceCommandHandler : ICommandTHandler<CreateInstockPriceCommand, Guid>
{
    private readonly IInstockPriceRepository _priceRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public CreateInstockPriceCommandHandler(
        IInstockPriceRepository priceRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _priceRepository = priceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreateInstockPriceCommand request, CancellationToken cancellationToken)
    {
        var existingByPriority = await _priceRepository.GetByPriorityAsync(request.Priority, cancellationToken);
        if (existingByPriority is not null)
        {
            return Result.Failure<Guid>(InstockPriceError.DuplicatePriority(request.Priority));
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var priceResult = InstockPrice.Create(
                request.Name,
                request.EffectiveFrom,
                request.EffectiveTo,
                request.Priority,
                request.IsActive);

            if (priceResult.IsFailure)
            {
                return Result.Failure<Guid>(priceResult.Error);
            }

            var price = priceResult.Value;
            _priceRepository.Add(price);

            return Result.Success(price.Id.Value);
        }, cancellationToken);
    }
}
