using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Queries.GetCapabilitiesByTopicAndMaterial;

internal sealed class GetCapabilitiesByTopicAndMaterialQueryHandler
    : IQueryHandler<GetCapabilitiesByTopicAndMaterialQuery, List<GetCapabilityBasicResponseDto>>
{
    private readonly ITopicMaterialCapabilityRepository _repository;
    private readonly ICapabilityRepository _capabilityRepository;

    public GetCapabilitiesByTopicAndMaterialQueryHandler(
        ITopicMaterialCapabilityRepository repository,
        ICapabilityRepository capabilityRepository)
    {
        _repository = repository;
        _capabilityRepository = capabilityRepository;
    }

    public async Task<ResultT<List<GetCapabilityBasicResponseDto>>> Handle(
        GetCapabilitiesByTopicAndMaterialQuery request,
        CancellationToken cancellationToken)
    {
        // Get all active TopicMaterialCapabilities for this topic and material
        var topicMaterialCapabilities = await _repository.FindAsync(
            tmc => tmc.TopicId == TopicId.From(request.TopicId)
                && tmc.MaterialId == MaterialId.From(request.MaterialId)
                && tmc.IsActive,
            cancellationToken);

        if (!topicMaterialCapabilities.Any())
            return Result.Success(new List<GetCapabilityBasicResponseDto>());

        // Get all capability IDs from the results
        var capabilityIds = topicMaterialCapabilities
            .Select(tmc => tmc.CapabilityId)
            .Distinct()
            .ToList();

        // Fetch all capabilities with batch query
        var capabilities = await _capabilityRepository.FindAsync(
            c => capabilityIds.Contains(c.Id) && c.IsActive,
            cancellationToken);

        // Map to DTOs
        var capabilityDtos = capabilities
            .Select(c => new GetCapabilityBasicResponseDto(
                c.Id.Value,
                c.Name,
                c.Slug))
            .ToList();

        return Result.Success(capabilityDtos);
    }
}
