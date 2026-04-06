using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.SharedKernel.Application.Media;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignAssets.Queries.GetAssetsByRequestId;

internal sealed class GetCustomDesignAssetsByRequestIdQueryHandler : IQueryHandler<GetCustomDesignAssetsByRequestIdQuery, List<GetCustomDesignAssetsByRequestIdResponseDto>>
{
    private readonly ICustomDesignAssetRepository _assetRepository;
    private readonly ICustomDesignRequestRepository _requestRepository;
    private readonly IMediaAssetService _mediaAssetService;

    public GetCustomDesignAssetsByRequestIdQueryHandler(
        ICustomDesignAssetRepository assetRepository,
        ICustomDesignRequestRepository requestRepository,
        IMediaAssetService mediaAssetService)
    {
        _assetRepository = assetRepository;
        _requestRepository = requestRepository;
        _mediaAssetService = mediaAssetService;
    }

    public async Task<ResultT<List<GetCustomDesignAssetsByRequestIdResponseDto>>> Handle(
        GetCustomDesignAssetsByRequestIdQuery request,
        CancellationToken cancellationToken)
    {
        var requestId = CustomDesignRequestId.From(request.RequestId);

        // Check if request exists
        var requestResult = await _requestRepository.GetByIdAsync(requestId, cancellationToken);
        if (requestResult.IsFailure)
        {
            return Result.Failure<List<GetCustomDesignAssetsByRequestIdResponseDto>>(requestResult.Error);
        }

        // Get all assets for this request
        // Note: We use the repository pattern. If asset repository doesn't have GetByRequestId,
        // we can implement it or use the GetAllAsync and filter in memory
        var assets = await _assetRepository.GetAllAsync(cancellationToken);
        
        var requestAssets = assets
            .Where(a => a.CustomDesignRequestId == requestId)
            .OrderBy(a => a.Version)
            .ToList();

        var dtos = requestAssets.Select(a => new GetCustomDesignAssetsByRequestIdResponseDto(
            a.Id.Value,
            a.Code,
            a.Version,
            a.Status.ToString(),
            _mediaAssetService.BuildAssetUrls(a.MultiviewImages),
            _mediaAssetService.BuildAssetUrl(a.CompositeMultiviewImage),
            _mediaAssetService.BuildAssetUrl(a.Rough3DModel),
            a.Rough3DModelTaskId,
            a.CustomerPrompt,
            a.NormalizePrompt,
            a.IsNeedSupport,
            a.IsFinalDesign,
            a.CreatedAt,
            a.UpdatedAt)).ToList();

        return Result.Success(dtos);
    }
}


