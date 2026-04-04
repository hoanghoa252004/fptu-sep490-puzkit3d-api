using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Queries.GetAllRequirements;

internal sealed class GetAllCustomDesignRequirementsQueryHandler : IQueryHandler<GetAllCustomDesignRequirementsQuery, IEnumerable<GetAllCustomDesignRequirementsResponseDto>>
{
    private readonly ICustomDesignRequirementRepository _repository;

    public GetAllCustomDesignRequirementsQueryHandler(ICustomDesignRequirementRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<IEnumerable<GetAllCustomDesignRequirementsResponseDto>>> Handle(
        GetAllCustomDesignRequirementsQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<GetAllCustomDesignRequirementsResponseDto> requirements;

        if (request.OnlyActive)
        {
            var activeRequirements = await _repository.GetActiveAsync(cancellationToken);
            requirements = activeRequirements.Select(MapToDto).ToList();
        }
        else
        {
            var allRequirements = await _repository.GetAllAsync(cancellationToken);
            requirements = allRequirements.Select(MapToDto).ToList();
        }

        return Result.Success(requirements);
    }

    private static GetAllCustomDesignRequirementsResponseDto MapToDto(Domain.Entities.CustomDesignRequirements.CustomDesignRequirement requirement)
    {
        return new GetAllCustomDesignRequirementsResponseDto(
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
            requirement.UpdatedAt);
    }
}
