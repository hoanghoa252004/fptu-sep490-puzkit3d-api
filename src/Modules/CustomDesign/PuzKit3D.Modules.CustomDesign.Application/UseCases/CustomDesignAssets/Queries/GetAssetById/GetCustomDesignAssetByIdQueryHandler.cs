using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;
using PuzKit3D.SharedKernel.Application.Media;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignAssets.Queries.GetAssetById;

internal sealed class GetCustomDesignAssetByIdQueryHandler : IQueryHandler<GetCustomDesignAssetByIdQuery, GetCustomDesignAssetByIdResponseDto>
{
    private readonly ICustomDesignAssetRepository _repository;
    private readonly IMediaAssetService _mediaAssetService;

    public GetCustomDesignAssetByIdQueryHandler(ICustomDesignAssetRepository repository, IMediaAssetService mediaAssetService)
    {
        _repository = repository;
        _mediaAssetService = mediaAssetService;
    }

    public async Task<ResultT<GetCustomDesignAssetByIdResponseDto>> Handle(
        GetCustomDesignAssetByIdQuery request,
        CancellationToken cancellationToken)
    {
        var assetResult = await _repository.GetByIdAsync(
            CustomDesignAssetId.From(request.Id),
            cancellationToken);

        if (assetResult.IsFailure)
            return Result.Failure<GetCustomDesignAssetByIdResponseDto>(assetResult.Error);

        var asset = assetResult.Value;

        var response = new GetCustomDesignAssetByIdResponseDto(
            asset.Id.Value,
            asset.Code,
            asset.CustomDesignRequestId.Value,
            asset.Version,
            asset.Status.ToString(),
            _mediaAssetService.BuildAssetUrls(asset.MultiviewImages),
            _mediaAssetService.BuildAssetUrl(asset.CompositeMultiviewImage),
            _mediaAssetService.BuildAssetUrl(asset.Rough3DModel),
            asset.Rough3DModelTaskId,
            asset.CustomerPrompt,
            asset.NormalizePrompt,
            asset.IsNeedSupport,
            asset.IsFinalDesign,
            asset.CreatedAt,
            asset.UpdatedAt);

        return Result.Success(response);
    }
}
