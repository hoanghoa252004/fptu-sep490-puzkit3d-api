using System.Text.Json;
using PuzKit3D.Modules.InStock.Application.Repositories;
using PuzKit3D.Modules.InStock.Application.Services;
using PuzKit3D.Modules.InStock.Application.UnitOfWork;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.InStock.Application.UseCases.InstockProducts.Commands.CreateInstockProduct;

internal sealed class CreateInstockProductCommandHandler : ICommandTHandler<CreateInstockProductCommand, Guid>
{
    private readonly IInstockProductRepository _productRepository;
    private readonly IInstockProductCodeGenerator _codeGenerator;
    private readonly IInStockUnitOfWork _unitOfWork;

    public CreateInstockProductCommandHandler(
        IInstockProductRepository productRepository,
        IInstockProductCodeGenerator codeGenerator,
        IInStockUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _codeGenerator = codeGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreateInstockProductCommand request, CancellationToken cancellationToken)
    {
        var existingBySlug = await _productRepository.GetBySlugAsync(request.Slug, cancellationToken);
        if (existingBySlug is not null)
        {
            return Result.Failure<Guid>(InstockProductError.DuplicateSlug(request.Slug));
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var code = await _codeGenerator.GenerateNextCodeAsync(cancellationToken);
            var previewAssetJson = JsonSerializer.Serialize(request.PreviewAsset);

            var productResult = InstockProduct.Create(
                code,
                request.Slug,
                request.Name,
                request.TotalPieceCount,
                request.DifficultLevel,
                request.EstimatedBuildTime,
                request.ThumbnailUrl,
                previewAssetJson,
                request.TopicId,
                request.AssemblyMethodId,
                request.CapabilityId,
                request.MaterialId,
                request.Description,
                request.IsActive);

            if (productResult.IsFailure)
            {
                return Result.Failure<Guid>(productResult.Error);
            }

            var product = productResult.Value;
            _productRepository.Add(product);

            return Result.Success(product.Id.Value);
        }, cancellationToken);
    }
}


