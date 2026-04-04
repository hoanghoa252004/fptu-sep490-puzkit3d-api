using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Queries.GetRequirementById;

internal sealed class GetCustomDesignRequirementByIdQueryHandler : IQueryHandler<GetCustomDesignRequirementByIdQuery, GetCustomDesignRequirementByIdResponseDto>
{
    private readonly ICustomDesignRequirementRepository _repository;

    public GetCustomDesignRequirementByIdQueryHandler(ICustomDesignRequirementRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<GetCustomDesignRequirementByIdResponseDto>> Handle(
        GetCustomDesignRequirementByIdQuery request,
        CancellationToken cancellationToken)
    {
        var requirementResult = await _repository.GetByIdAsync(
            CustomDesignRequirementId.From(request.Id),
            cancellationToken);

        if (requirementResult.IsFailure)
        {
            return Result.Failure<GetCustomDesignRequirementByIdResponseDto>(requirementResult.Error);
        }

        var responseDto = new GetCustomDesignRequirementByIdResponseDto(
            requirementResult.Value.Id.Value,
            requirementResult.Value.Code,
            requirementResult.Value.TopicId,
            requirementResult.Value.MaterialId,
            requirementResult.Value.AssemblyMethodId,
            requirementResult.Value.Difficulty.ToString(),
            requirementResult.Value.MinPartQuantity,
            requirementResult.Value.MaxPartQuantity,
            requirementResult.Value.IsActive,
            requirementResult.Value.CreatedAt,
            requirementResult.Value.UpdatedAt);

        return Result.Success(responseDto);
    }
}
