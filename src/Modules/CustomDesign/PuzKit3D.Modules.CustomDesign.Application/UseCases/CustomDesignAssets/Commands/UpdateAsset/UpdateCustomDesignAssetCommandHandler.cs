using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.Modules.CustomDesign.Application.UnitOfWork;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignAssets.Commands.UpdateAsset;

internal sealed class UpdateCustomDesignAssetCommandHandler : ICommandHandler<UpdateCustomDesignAssetCommand>
{
    private readonly ICustomDesignAssetRepository _repository;
    private readonly ICustomDesignUnitOfWork _customDesignUnitOfWork;

    public UpdateCustomDesignAssetCommandHandler(ICustomDesignAssetRepository repository, ICustomDesignUnitOfWork customDesignUnitOfWork)
    {
        _repository = repository;
        _customDesignUnitOfWork = customDesignUnitOfWork;
    }

    public async Task<Result> Handle(
        UpdateCustomDesignAssetCommand request,
        CancellationToken cancellationToken)
    {
        var assetResult = await _repository.GetByIdAsync(
            CustomDesignAssetId.From(request.Id),
            cancellationToken);

        if (assetResult.IsFailure)
            return Result.Failure(assetResult.Error);

        var asset = assetResult.Value;

        // Handle MultiviewImages update
        string? multiviewImages = asset.MultiviewImages;
        if (request.MultiviewImages != null)
        {
            multiviewImages = string.Join(",", request.MultiviewImages);
        }

        // Handle IsNeedSupport update with validation
        bool isNeedSupport = asset.IsNeedSupport;
        if (request.IsNeedSupport.HasValue)
        {
            if (asset.IsNeedSupport == request.IsNeedSupport.Value)
            {
                return Result.Failure(Error.Validation(
                    "SAME_VALUE",
                    $"IsNeedSupport is already set to {request.IsNeedSupport.Value}"));
            }
            isNeedSupport = request.IsNeedSupport.Value;
        }

        // Handle IsFinalDesign update with validation
        bool isFinalDesign = asset.IsFinalDesign;
        if (request.IsFinalDesign.HasValue)
        {
            if (asset.IsFinalDesign == request.IsFinalDesign.Value)
            {
                return Result.Failure(Error.Validation(
                    "SAME_VALUE",
                    $"IsFinalDesign is already set to {request.IsFinalDesign.Value}"));
            }
            isFinalDesign = request.IsFinalDesign.Value;
        }

        return await _customDesignUnitOfWork.ExecuteAsync(async () =>
        {
            // Update asset
            asset.Update(
                multiviewImages: multiviewImages,
                compositeMultiviewImage: asset.CompositeMultiviewImage,
                rough3DModel: asset.Rough3DModel,
                rough3DModelTaskId: asset.Rough3DModelTaskId,
                customerPrompt: asset.CustomerPrompt,
                normalizePrompt: asset.NormalizePrompt,
                isNeedSupport: isNeedSupport,
                isFinalDesign: isFinalDesign,
                updatedAt: DateTime.UtcNow);

            _repository.Update(asset);

            return Result.Success();
        }, cancellationToken);
    }
}


