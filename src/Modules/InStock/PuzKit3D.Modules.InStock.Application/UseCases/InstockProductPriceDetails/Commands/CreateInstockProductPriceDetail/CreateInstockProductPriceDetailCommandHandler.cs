using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductPriceDetails.Commands.CreateInstockProductPriceDetail;

internal sealed class CreateInstockProductPriceDetailCommandHandler : ICommandTHandler<CreateInstockProductPriceDetailCommand, Guid>
{
    private readonly IInstockPriceRepository _priceRepository;
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly IInstockProductPriceDetailRepository _priceDetailRepository;
    private readonly IInStockUnitOfWork _unitOfWork;

    public CreateInstockProductPriceDetailCommandHandler(
        IInstockPriceRepository priceRepository,
        IInstockProductVariantRepository variantRepository,
        IInstockProductPriceDetailRepository priceDetailRepository,
        IInStockUnitOfWork unitOfWork)
    {
        _priceRepository = priceRepository;
        _variantRepository = variantRepository;
        _priceDetailRepository = priceDetailRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreateInstockProductPriceDetailCommand request, CancellationToken cancellationToken)
    {
        var priceId = InstockPriceId.From(request.PriceId);
        var price = await _priceRepository.GetByIdAsync(priceId, cancellationToken);

        if (price is null)
        {
            return Result.Failure<Guid>(InstockPriceError.NotFound(request.PriceId));
        }

        var variantId = InstockProductVariantId.From(request.VariantId);
        var variant = await _variantRepository.GetByIdAsync(variantId, cancellationToken);

        if (variant is null)
        {
            return Result.Failure<Guid>(InstockProductVariantError.NotFound(request.VariantId));
        }

        var existingPriceDetail = await _priceDetailRepository.GetByPriceAndVariantAsync(
            request.PriceId, request.VariantId, cancellationToken);

        if (existingPriceDetail is not null)
        {
            return Result.Failure<Guid>(InstockProductPriceDetailError.DuplicatePriceDetail());
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var priceDetailResult = InstockProductPriceDetail.Create(
                priceId,
                variantId,
                request.UnitPrice,
                request.IsActive);

            if (priceDetailResult.IsFailure)
            {
                return Result.Failure<Guid>(priceDetailResult.Error);
            }

            var priceDetail = priceDetailResult.Value;
            _priceDetailRepository.Add(priceDetail);

            return Result.Success(priceDetail.Id.Value);
        }, cancellationToken);
    }
}
