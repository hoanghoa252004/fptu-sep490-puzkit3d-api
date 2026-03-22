using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Text.Json;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Commands.CreatePartnerProduct;

internal sealed class CreatePartnerProductCommandHandler : ICommandTHandler<CreatePartnerProductCommand, Guid>
{
    private readonly IPartnerProductRepository _partnerProductRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;

    public CreatePartnerProductCommandHandler(
        IPartnerProductRepository partnerProductRepository,
        IPartnerUnitOfWork unitOfWork)
    {
        _partnerProductRepository = partnerProductRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(
        CreatePartnerProductCommand request,
        CancellationToken cancellationToken)
    {
        var existingBySlug = await _partnerProductRepository.GetBySlugAsync(request.Slug, cancellationToken);
        if (existingBySlug is not null)
        {
            return Result.Failure<Guid>(PartnerProductError.DuplicateSlug(request.Slug));
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var previewAssetJson = JsonSerializer.Serialize(request.PreviewAsset);

            var result = PartnerProduct.Create(
            PartnerId.From(request.PartnerId),
            request.Name,
            request.ReferencePrice,
            request.ThumbnailUrl,
            previewAssetJson,
            request.Slug,
            request.Description,
            request.IsActive);

            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }

            var product = result.Value;
            _partnerProductRepository.Add(product);

            return Result.Success(product.Id.Value);
        }, cancellationToken);
    }
}
