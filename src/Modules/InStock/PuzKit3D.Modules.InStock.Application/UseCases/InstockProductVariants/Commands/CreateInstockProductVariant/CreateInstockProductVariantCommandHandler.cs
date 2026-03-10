using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Services;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProductVariants.Commands.CreateInstockProductVariant;

internal sealed class CreateInstockProductVariantCommandHandler : ICommandTHandler<CreateInstockProductVariantCommand, Guid>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly IInstockProductVariantRepository _variantRepository;
    private readonly IInstockProductVariantSkuGenerator _skuGenerator;
    private readonly IInStockUnitOfWork _unitOfWork;

    public CreateInstockProductVariantCommandHandler(
        IInstockProductRepository productRepository,
        IInstockProductVariantRepository variantRepository,
        IInstockProductVariantSkuGenerator skuGenerator,
        IInStockUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _variantRepository = variantRepository;
        _skuGenerator = skuGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreateInstockProductVariantCommand request, CancellationToken cancellationToken)
    {
        var productId = InstockProductId.From(request.ProductId);
        var product = await _productRepository.GetByIdAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure<Guid>(InstockProductError.NotFound(request.ProductId));
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var sku = await _skuGenerator.GenerateNextSkuAsync(cancellationToken);

            var variantResult = InstockProductVariant.Create(
                productId,
                sku,
                request.Color,
                request.AssembledLengthMm,
                request.AssembledWidthMm,
                request.AssembledHeightMm,
                request.IsActive);

            if (variantResult.IsFailure)
            {
                return Result.Failure<Guid>(variantResult.Error);
            }

            var variant = variantResult.Value;
            _variantRepository.Add(variant);

            return Result.Success(variant.Id.Value);
        }, cancellationToken);
    }
}

