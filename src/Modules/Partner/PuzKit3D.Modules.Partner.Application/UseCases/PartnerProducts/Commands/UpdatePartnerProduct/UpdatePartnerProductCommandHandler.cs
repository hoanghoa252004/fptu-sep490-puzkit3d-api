using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Application.UnitOfWork;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;
using System.Text.Json;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProducts.Commands.UpdatePartnerProduct;

internal sealed class UpdatePartnerProductCommandHandler : ICommandHandler<UpdatePartnerProductCommand>
{
    private readonly IPartnerProductRepository _partnerProductRepository;
    private readonly IPartnerUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public UpdatePartnerProductCommandHandler(
        IPartnerProductRepository partnerProductRepository,
        IPartnerUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _partnerProductRepository = partnerProductRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
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

        var isManager = _currentUser.IsInRole(Roles.BusinessManager);
        var isStaff = _currentUser.IsInRole(Roles.Staff);

        if (isStaff && request.ReferencePrice != product.ReferencePrice)
        {
            return Result.Failure(Error.Failure("PartnerProduct.UpdateError",
                "Staff cannot change ReferencePrice. Only Managers can update this field."));
        }

        var previewAssetJson = JsonSerializer.Serialize(request.PreviewAsset);

        return await _unitOfWork.ExecuteAsync<Result>(async () =>
        {
            var finalReferencePrice = isManager ? request.ReferencePrice : product.ReferencePrice;

            var result = product.Update(
            request.Name,
            finalReferencePrice,
            request.Quantity,
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
