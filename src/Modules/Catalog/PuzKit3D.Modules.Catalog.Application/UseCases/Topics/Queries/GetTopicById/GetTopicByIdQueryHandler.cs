using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Topics.Queries.GetTopicById;

internal sealed class GetTopicByIdQueryHandler : IQueryHandler<GetTopicByIdQuery, object>
{
    private readonly ITopicRepository _topicRepository;
    private readonly ICurrentUser _currentUser;

    public GetTopicByIdQueryHandler(
        ITopicRepository topicRepository,
        ICurrentUser currentUser)
    {
        _topicRepository = topicRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<object>> Handle(
        GetTopicByIdQuery request,
        CancellationToken cancellationToken)
    {
        var topic = await _topicRepository.GetByIdAsync(TopicId.From(request.Id), cancellationToken);

        if (topic is null)
        {
            return Result.Failure<object>(TopicError.NotFound(request.Id));
        }

        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // For non-staff/manager users, only show active topics
        if (!isStaffOrManager && !topic.IsActive)
        {
            return Result.Failure<object>(TopicError.NotFound(request.Id));
        }

         // Build response DTO based on user role
         object topicDto = isStaffOrManager
             ? new GetTopicDetailedResponseDto(
                 topic.Id.Value,
                 topic.Name,
                 topic.Slug,
                 topic.ParentId?.Value,
                 topic.Description,
                 topic.IsActive,
                 topic.CreatedAt,
                 topic.UpdatedAt)
             : new GetTopicResponseDto(
                 topic.Id.Value,
                 topic.Name,
                 topic.Slug,
                 topic.ParentId?.Value,
                 topic.Description);
        return Result.Success(topicDto);
    }
}
