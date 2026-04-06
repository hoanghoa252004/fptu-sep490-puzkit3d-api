using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Application.Media;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequests.Queries.GetAllRequests;

internal sealed class GetAllCustomDesignRequestsQueryHandler : IQueryHandler<GetAllCustomDesignRequestsQuery, PagedResult<GetAllCustomDesignRequestsResponseDto>>
{
    private readonly ICustomDesignRequestRepository _repository;
    private readonly ICurrentUser _currentUser;
    private readonly IMediaAssetService _mediaAssetService;

    public GetAllCustomDesignRequestsQueryHandler(
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

    public async Task<ResultT<PagedResult<GetAllCustomDesignRequestsResponseDto>>> Handle(
        GetAllCustomDesignRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var allRequests = await _repository.GetAllAsync(cancellationToken);

        // Filter by customer if user is customer
        if (_currentUser.IsInRole(Roles.Customer))
        {
            var customerId = Guid.Parse(_currentUser.UserId!);
            allRequests = allRequests.Where(r => r.CustomerId == customerId).ToList();
        }

        // Filter by status if provided and sort by CreatedAt descending
        var query = allRequests.AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (Enum.TryParse<CustomDesignRequestStatus>(request.Status, true, out var statusEnum))
            {
                query = query.Where(r => r.Status == statusEnum);
            }
        }

        // Sort by CreatedAt descending
        query = query.OrderByDescending(r => r.CreatedAt);

        // Get total count before paging
        var totalCount = query.Count();

        // Apply pagination
        var items = query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(r => new GetAllCustomDesignRequestsResponseDto(
                r.Id.Value,
                r.Code,
                r.CustomerId,
                r.CustomDesignRequirementId.Value,
                r.DesiredLengthMm,
                r.DesiredWidthMm,
                r.DesiredHeightMm,
                BuildSketchesUrls(r.Sketches),
                r.CustomerPrompt,
                r.DesiredDeliveryDate,
                r.DesiredQuantity,
                r.TargetBudget,
                r.UsedSupportConceptDesignTime,
                r.Status.ToString(),
                r.Type.ToString(),
                r.Note,
                r.CreatedAt,
                r.UpdatedAt))
            .ToList();

        var pagedResult = PagedResult<GetAllCustomDesignRequestsResponseDto>.Create(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}

