using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Text.Json;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Commands.UpdatePartnerProduct;

internal sealed class UpdatePartnerProductCommandHandler : ICommandHandler<UpdatePartnerProductCommand>
{
    private readonly IPartnerProductRepository _partnerProductRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;

    public UpdatePartnerProductCommandHandler(
        IPartnerProductRepository partnerProductRepository,
        IPartnerUnitOfWork unitOfWork)
    {
        _partnerProductRepository = partnerProductRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdatePartnerProductCommand request,
        CancellationToken cancellationToken)
    {
        var productId = PartnerProductId.From(request.Id);
        var product = await _partnerProductRepository.GetByIdAsync(productId, cancellationToken);

        if (product is null)
        {
            return Result.Failure(PartnerProductError.NotFound(request.Id));
        }

        var existingProduct = await _partnerProductRepository.GetBySlugAsync(request.Slug, cancellationToken);
        if (existingProduct is not null && existingProduct.Id != productId)
        {
            return Result.Failure(PartnerProductError.DuplicateSlug(request.Slug));
        }

        var previewAssetJson = JsonSerializer.Serialize(request.PreviewAsset);

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            var result = product.Update(
            request.Name,
            request.ReferencePrice,
            request.ThumbnailUrl,
            previewAssetJson,
            request.Slug,
            request.Description);

            if (result.IsFailure)
            {
                return result;
            }

            _partnerProductRepository.Update(product);

            return Result.Success();
        }, cancellationToken);
        
    }
}
