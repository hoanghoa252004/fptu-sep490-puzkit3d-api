using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Media;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequests.Queries.GetRequestById;

internal sealed class GetCustomDesignRequestByIdQueryHandler : IQueryHandler<GetCustomDesignRequestByIdQuery, GetCustomDesignRequestByIdResponseDto>
{
    private readonly ICustomDesignRequestRepository _repository;
    private readonly ICurrentUser _currentUser;
    private readonly IMediaAssetService _mediaAssetService;

    public GetCustomDesignRequestByIdQueryHandler(
        ICustomDesignRequestRepository repository,
        ICurrentUser currentUser,
        IMediaAssetService mediaAssetService)
    {
        _repository = repository;
        _currentUser = currentUser;
        _mediaAssetService = mediaAssetService;
    }

    private List<string>? BuildSketchesUrls(string? sketches)
    {
        if (string.IsNullOrWhiteSpace(sketches))
            return null;

        var urls = _mediaAssetService.BuildAssetUrls(sketches);
        return urls.Count > 0 ? urls : null;
    }

    public async Task<ResultT<GetCustomDesignRequestByIdResponseDto>> Handle(
        GetCustomDesignRequestByIdQuery request,
        CancellationToken cancellationToken)
    {
        var customDesignRequestResult = await _repository.GetByIdAsync(
            CustomDesignRequestId.From(request.Id),
            cancellationToken);

        if (customDesignRequestResult.IsFailure)
            return Result.Failure<GetCustomDesignRequestByIdResponseDto>(customDesignRequestResult.Error);

        var customDesignRequest = customDesignRequestResult.Value;

        // Check authorization: customers can only view their own requests
        if (_currentUser.IsInRole(Roles.Customer))
        {
            var customerId = Guid.Parse(_currentUser.UserId!);
            if (customDesignRequest.CustomerId != customerId)
                return Result.Failure<GetCustomDesignRequestByIdResponseDto>(CustomDesignRequestError.NotFound());
        }

        var response = new GetCustomDesignRequestByIdResponseDto(
            customDesignRequest.Id.Value,
            customDesignRequest.Code,
            customDesignRequest.CustomerId,
            customDesignRequest.CustomDesignRequirementId.Value,
            customDesignRequest.DesiredLengthMm,
            customDesignRequest.DesiredWidthMm,
            customDesignRequest.DesiredHeightMm,
            BuildSketchesUrls(customDesignRequest.Sketches),
            customDesignRequest.CustomerPrompt,
            customDesignRequest.DesiredDeliveryDate,
            customDesignRequest.DesiredQuantity,
            customDesignRequest.TargetBudget,
            customDesignRequest.UsedSupportConceptDesignTime,
            customDesignRequest.Status.ToString(),
            customDesignRequest.Type.ToString(),
            customDesignRequest.CreatedAt,
            customDesignRequest.UpdatedAt);

        return Result.Success(response);
    }
}


