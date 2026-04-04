using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Application.UnitOfWork;
using PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Queries.GetRequirementById;
using PuzKit3D.Modules.CustomDesign.Domain.Entities;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Commands.UpdateRequirement;

internal sealed class UpdateCustomDesignRequirementCommandHandler : ICommandHandler<UpdateCustomDesignRequirementCommand>
{
    private readonly ICustomDesignRequirementRepository _repository;
    private readonly ICustomDesignUnitOfWork _unitOfWork;
    private readonly ITopicReplicaRepository _topicReplicaRepository;
    private readonly IMaterialReplicaRepository _materialReplicaRepository;
    private readonly IAssemblyMethodReplicaRepository _assemblyMethodReplicaRepository;

    public UpdateCustomDesignRequirementCommandHandler(
        ICustomDesignRequirementRepository repository,
        ICustomDesignUnitOfWork unitOfWork,
        ITopicReplicaRepository topicReplicaRepository,
        IMaterialReplicaRepository materialReplicaRepository,
        IAssemblyMethodReplicaRepository assemblyMethodReplicaRepository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _topicReplicaRepository = topicReplicaRepository;
        _materialReplicaRepository = materialReplicaRepository;
        _assemblyMethodReplicaRepository = assemblyMethodReplicaRepository;
    }

    public async Task<Result> Handle(
        UpdateCustomDesignRequirementCommand request,
        CancellationToken cancellationToken)
    {
        var requirementResult = await _repository.GetByIdAsync(
            CustomDesignRequirementId.From(request.Id),
            cancellationToken);

        if (requirementResult.IsFailure)
        {
            return Result.Failure<GetCustomDesignRequirementByIdResponseDto>(requirementResult.Error);
        }

        // Validate Topic if provided
        if (request.TopicId.HasValue)
        {
            var topic = await _topicReplicaRepository.GetByIdAsync(request.TopicId.Value, cancellationToken);
            if (topic is null)
                return Result.Failure(CustomDesignRequirementError.TopicNotFound(request.TopicId.Value));
        }

        // Validate Material if provided
        if (request.MaterialId.HasValue)
        {
            var material = await _materialReplicaRepository.GetByIdAsync(request.MaterialId.Value, cancellationToken);
            if (material is null)
                return Result.Failure(CustomDesignRequirementError.MaterialNotFound(request.MaterialId.Value));
        }

        // Validate AssemblyMethod if provided
        if (request.AssemblyMethodId.HasValue)
        {
            var assemblyMethod = await _assemblyMethodReplicaRepository.GetByIdAsync(request.AssemblyMethodId.Value, cancellationToken);
            if (assemblyMethod is null)
                return Result.Failure(CustomDesignRequirementError.AssemblyMethodNotFound(request.AssemblyMethodId.Value));
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Update only provided fields
            var updateResult = requirementResult.Value.Update(
                request.TopicId,
                request.MaterialId,
                request.AssemblyMethodId,
                request.Difficulty,
                request.MinPartQuantity,
                request.MaxPartQuantity,
                request.IsActive,
                DateTime.UtcNow);

            if (updateResult.IsFailure)
                return Result.Failure(updateResult.Error);

            _repository.Update(requirementResult.Value);

            return Result.Success();
        }, cancellationToken);
    }
}
