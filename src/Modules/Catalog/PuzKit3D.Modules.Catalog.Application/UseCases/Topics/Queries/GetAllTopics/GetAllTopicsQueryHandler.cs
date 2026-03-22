using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Queries.Shared;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Queries.GetAllTopics;

internal sealed class GetAllTopicsQueryHandler
    : IQueryHandler<GetAllTopicsQuery, PagedResult<object>>
{
    private readonly ITopicRepository _topicRepository;
    private readonly ICurrentUser _currentUser;

    public GetAllTopicsQueryHandler(
        ITopicRepository topicRepository,
        ICurrentUser currentUser)
    {
        _topicRepository = topicRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetAllTopicsQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole("Staff") || _currentUser.IsInRole("Business Manager"));

        // Get all topics
        var allTopics = await _topicRepository.GetAllAsync(cancellationToken);
        var query = allTopics.AsQueryable();

        // For non-staff/manager users (anonymous or customer), only show active items
        if (!isStaffOrManager)
        {
            query = query.Where(t => t.IsActive);
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(t =>
                t.Name.ToLower().Contains(searchTerm) ||
                t.Slug.ToLower().Contains(searchTerm) ||
                (t.Description != null && t.Description.ToLower().Contains(searchTerm)));
        }

        // Apply parent id filter if provided
        if (request.ParentId.HasValue && request.ParentId.Value != Guid.Empty)
        {
            query = query.Where(t => t.ParentId.Value == request.ParentId.Value);
        }

        // Apply status filter if explicitly set
        if (request.IsActive.HasValue)
        {
            query = query.Where(t => t.IsActive == request.IsActive.Value);
        }

        // Apply pagination
        var totalCount = query.Count();
        var topics = query
            .OrderBy(t => t.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

         // Build response DTOs
         var topicDtos = isStaffOrManager
             ? topics.Select(t => (object)new GetTopicDetailedResponseDto(
                 t.Id.Value,
                 t.Name,
                 t.Slug,
                 t.ParentId?.Value,
                 t.Description,
                 t.IsActive,
                 t.CreatedAt,
                 t.UpdatedAt)).ToList()
             : topics.Select(t => (object)new GetTopicResponseDto(
                 t.Id.Value,
                 t.Name,
                 t.Slug,
                 t.ParentId?.Value,
                 t.Description)).ToList();

        var pagedResult = new PagedResult<object>(
            topicDtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
