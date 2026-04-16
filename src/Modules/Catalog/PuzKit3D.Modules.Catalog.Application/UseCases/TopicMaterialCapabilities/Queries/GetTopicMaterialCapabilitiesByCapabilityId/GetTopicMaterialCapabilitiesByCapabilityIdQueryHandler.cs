using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Queries.GetTopicMaterialCapabilitiesByCapabilityId;

internal sealed class GetTopicMaterialCapabilitiesByCapabilityIdQueryHandler : IQueryHandler<GetTopicMaterialCapabilitiesByCapabilityIdQuery, object>
{
    private readonly ITopicMaterialCapabilityRepository _repository;
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly ITopicRepository _topicRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly ICurrentUser _currentUser;

    public GetTopicMaterialCapabilitiesByCapabilityIdQueryHandler(
        ITopicMaterialCapabilityRepository repository,
        ICapabilityRepository capabilityRepository,
        ITopicRepository topicRepository,
        IMaterialRepository materialRepository,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _capabilityRepository = capabilityRepository;
        _topicRepository = topicRepository;
        _materialRepository = materialRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<object>> Handle(
        GetTopicMaterialCapabilitiesByCapabilityIdQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // Verify capability exists
        var capabilityId = CapabilityId.From(request.CapabilityId);
        var capability = await _capabilityRepository.GetByIdAsync(capabilityId, cancellationToken);

        if (capability is null)
        {
            return Result.Failure<object>(
                CapabilityError.NotFound(request.CapabilityId));
        }

        // For non-staff/manager users, only return if capability is active
        if (!isStaffOrManager && !capability.IsActive)
        {
            return Result.Failure<object>(
                CapabilityError.NoPermissionGranted());
        }

        // Get topic material capabilities for this capability
        var items = await _repository.FindAsync(
            tmc => tmc.CapabilityId == capabilityId,
            cancellationToken);

        var itemList = items.ToList();

        // Get list TopicIds and MaterialIds to minimize queries later
        var topicIds = itemList.Select(x => x.TopicId).Distinct().ToList();
        var materialIds = itemList.Select(x => x.MaterialId).Distinct().ToList();

        // Get topics and materials
        var topics = await _topicRepository.FindAsync(
            t => topicIds.Contains(t.Id),
            cancellationToken);

        var materials = await _materialRepository.FindAsync(
            m => materialIds.Contains(m.Id),
            cancellationToken);

        // Create dictionaries for quick lookup
        var topicDict = topics.ToDictionary(x => x.Id);
        var materialDict = materials.ToDictionary(x => x.Id);

        var responses = new List<object>();

        foreach (var tmc in itemList)
        {
            // For non-staff/manager users, skip inactive items
            if (!isStaffOrManager && !tmc.IsActive)
                continue;

            if (!topicDict.TryGetValue(tmc.TopicId, out var topic) ||
                !materialDict.TryGetValue(tmc.MaterialId, out var material))
                continue;

            if (isStaffOrManager)
            {
                responses.Add(new GetTopicMaterialCapabilityDetailedResponseDto(
                    tmc.Id.Value,
                    new(
                        topic.Id.Value,
                        topic.Name,
                        topic.Slug,
                        topic.ParentId?.Value,
                        topic.Description,
                        topic.FactorPercentage,
                        topic.IsActive,
                        topic.CreatedAt,
                        topic.UpdatedAt),
                    new(
                        material.Id.Value,
                        material.Name,
                        material.Slug,
                        material.Description,
                        material.FactorPercentage,
                        material.BasePrice,
                        material.IsActive,
                        material.CreatedAt,
                        material.UpdatedAt),
                    new(
                        capability.Id.Value,
                        capability.Name,
                        capability.Slug,
                        capability.Description,
                        capability.FactorPercentage,
                        capability.IsActive,
                        capability.CreatedAt,
                        capability.UpdatedAt),
                    tmc.IsActive));
            }
            else
            {
                responses.Add(new GetTopicMaterialCapabilityResponseDto(
                    tmc.Id.Value,
                    new(
                        topic.Id.Value,
                        topic.Name,
                        topic.Slug,
                        null,
                        null,
                        topic.FactorPercentage),
                    new(
                        material.Id.Value,
                        material.Name,
                        material.Slug,
                        null,
                        material.FactorPercentage,
                        material.BasePrice),
                    new(
                        capability.Id.Value,
                        capability.Name,
                        capability.Slug,
                        null,
                        capability.FactorPercentage)));
            }
        }

        return Result.Success((object)responses);
    }
}
