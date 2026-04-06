using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Application.Services;
using PuzKit3D.Modules.CustomDesign.Application.UnitOfWork;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets.DomainEvents;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignAssets.Commands.CreateAsset;

internal sealed class CreateCustomDesignAssetCommandHandler : ICommandTHandler<CreateCustomDesignAssetCommand, Guid>
{
    private readonly ICustomDesignAssetRepository _assetRepository;
    private readonly ICustomDesignRequestRepository _requestRepository;
    private readonly ICustomDesignAssetCodeGenerator _codeGenerator;
    private readonly ICustomDesignUnitOfWork _uow;

    public CreateCustomDesignAssetCommandHandler(
        ICustomDesignAssetRepository assetRepository,
        ICustomDesignRequestRepository requestRepository,
        ICustomDesignAssetCodeGenerator codeGenerator,
        ICustomDesignUnitOfWork uow)
    {
        _assetRepository = assetRepository;
        _requestRepository = requestRepository;
        _codeGenerator = codeGenerator;
        _uow = uow;
    }

    public async Task<ResultT<Guid>> Handle(
        CreateCustomDesignAssetCommand request,
        CancellationToken cancellationToken)
    {
        var requestId = CustomDesignRequestId.From(request.RequestId);

        // Check if request exists
        var requestResult = await _requestRepository.GetByIdAsync(requestId, cancellationToken);
        if (requestResult.IsFailure)
            return Result.Failure<Guid>(requestResult.Error);

        var designRequest = requestResult.Value;

        // Check if UsedSupportConceptDesignTime has reached max (3)
        if (designRequest.UsedSupportConceptDesignTime >= 3)
        {
            return Result.Failure<Guid>(
                Error.Validation(
                    "CustomDesignAsset.MaxFreeAssetsExceeded",
                    "Maximum number of free custom design assets (3) has been reached"));
        }

        // Get all assets for this request
        var allAssets = await _assetRepository.GetAllAsync(cancellationToken);
        var requestAssets = allAssets
            .Where(a => a.CustomDesignRequestId == requestId)
            .OrderByDescending(a => a.Version)
            .ToList();

        // Check if there are any assets for this request
        if (requestAssets.Count == 0)
        {
            return Result.Failure<Guid>(
                Error.Validation(
                    "CustomeDesignAsset.BaseAssetIsGenerating",
                    "Base asset (version 0) is generating, please wait when completed and try a gain"));
        }

        // Check if all existing assets are completed
        var allAssetsCompleted = requestAssets.All(a => a.Status == CustomDesignAssetStatus.Completed);
        if (!allAssetsCompleted)
        {
            return Result.Failure<Guid>(
                Error.Validation(
                    "CustomDesignAsset.PreviousAssetsNotCompleted",
                    "All previous assets must be completed before creating a new version"));
        }

        int nextVersion = requestAssets.First().Version + 1;

        return await _uow.ExecuteAsync(async () =>
        {
            // Generate code
            var code = await _codeGenerator.GenerateCodeAsync(cancellationToken);

            // Create asset
            var assetId = Guid.NewGuid();
            var asset = CustomDesignAsset.Create(
                id: assetId,
                code: code,
                customDesignRequestId: request.RequestId,
                version: nextVersion,
                multiviewImages: null,
                compositeMultiviewImage: null,
                rough3DModel: null,
                rough3DModelTaskId: null,
                customerPrompt: request.CustomerPrompt,
                normalizePrompt: null,
                isNeedSupport: false,
                isFinalDesign: false,
                createdAt: DateTime.UtcNow,
                updatedAt: DateTime.UtcNow);

            // Add asset to repository
            await _assetRepository.AddAsync(asset, cancellationToken);

            return Result.Success(assetId);
        });
    }
}

