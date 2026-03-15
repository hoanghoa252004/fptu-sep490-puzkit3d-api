using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockPrices.Commands.DeleteInstockPrice;

internal sealed class DeleteInstockPriceCommandHandler : ICommandHandler<DeleteInstockPriceCommand>
{
    private readonly IInstockPriceRepository _priceRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public DeleteInstockPriceCommandHandler(
        IInstockPriceRepository priceRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _priceRepository = priceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteInstockPriceCommand request, CancellationToken cancellationToken)
    {
        var priceId = InstockPriceId.From(request.PriceId);
        var price = await _priceRepository.GetByIdAsync(priceId, cancellationToken);

        if (price is null)
        {
            return Result.Failure(InstockPriceError.NotFound(request.PriceId));
        }

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            price.Delete();
            _priceRepository.Delete(price);

            return Result.Success();
        }, cancellationToken);
    }
}
