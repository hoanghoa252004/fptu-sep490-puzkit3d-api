using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Queries.GetAllRequirements;

internal sealed class GetAllCustomDesignRequirementsQueryHandler : IQueryHandler<GetAllCustomDesignRequirementsQuery, IEnumerable<GetAllCustomDesignRequirementsResponseDto>>
{
    private readonly ICustomDesignRequirementRepository _repository;
    private readonly IRequirementCapabilityDetailRepository _capabilityDetailRepository;

    public GetAllCustomDesignRequirementsQueryHandler(
        ICustomDesignRequirementRepository repository,
        IRequirementCapabilityDetailRepository capabilityDetailRepository)
    {
        _repository = repository;
        _capabilityDetailRepository = capabilityDetailRepository;
    }

    public async Task<ResultT<IEnumerable<GetAllCustomDesignRequirementsResponseDto>>> Handle(
        GetAllCustomDesignRequirementsQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<GetAllCustomDesignRequirementsResponseDto> requirements;

        if (request.OnlyActive)
        {
            var activeRequirements = await _repository.GetActiveAsync(cancellationToken);
            requirements = await MapToDto(activeRequirements, cancellationToken);
        }
        else
        {
            var allRequirements = await _repository.GetAllAsync(cancellationToken);
            requirements = await MapToDto(allRequirements, cancellationToken);
        }

        return Result.Success(requirements);
    }

    private async Task<IEnumerable<GetAllCustomDesignRequirementsResponseDto>> MapToDto(
        IEnumerable<Domain.Entities.CustomDesignRequirements.CustomDesignRequirement> requirements,
        CancellationToken cancellationToken)
    {
        var result = new List<GetAllCustomDesignRequirementsResponseDto>();

        foreach (var requirement in requirements)
        {
            var capabilities = await _capabilityDetailRepository.GetByRequirementIdAsync(
                requirement.Id,
                cancellationToken);

            var dto = new GetAllCustomDesignRequirementsResponseDto(
                requirement.Id.Value,
                requirement.Code,
                requirement.TopicId,
                requirement.MaterialId,
                requirement.AssemblyMethodId,
                requirement.Difficulty.ToString(),
                requirement.MinPartQuantity,
                requirement.MaxPartQuantity,
                requirement.IsActive,
                requirement.CreatedAt,
                requirement.UpdatedAt,
                capabilities.Select(c => c.CapabilityId).ToList());

            result.Add(dto);
        }

        return result;
    }
}
