using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Application.Services;
using PuzKit3D.Modules.CustomDesign.Application.UnitOfWork;
using PuzKit3D.Modules.CustomDesign.Domain.Entities;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.RequirementCapabilityDetails;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Commands.CreateRequirement;

internal sealed class CreateCustomDesignRequirementCommandHandler : ICommandTHandler<CreateCustomDesignRequirementCommand, Guid>
{
    private readonly ICustomDesignRequirementRepository _repository;
    private readonly ICustomDesignRequirementCodeGenerator _codeGenerator;
    private readonly IRequirementCapabilityDetailRepository _capabilityDetailRepository;
    private readonly ICustomDesignUnitOfWork _unitOfWork;
    private readonly ITopicReplicaRepository _topicReplicaRepository;
    private readonly IMaterialReplicaRepository _materialReplicaRepository;
    private readonly IAssemblyMethodReplicaRepository _assemblyMethodReplicaRepository;
    private readonly ICapabilityReplicaRepository _capabilityReplicaRepository;
    public CreateCustomDesignRequirementCommandHandler(
        ICustomDesignRequirementRepository repository,
        ICustomDesignRequirementCodeGenerator codeGenerator,
        IRequirementCapabilityDetailRepository capabilityDetailRepository,
        ICustomDesignUnitOfWork unitOfWork,
        ITopicReplicaRepository topicReplicaRepository,
        IMaterialReplicaRepository materialReplicaRepository,
        IAssemblyMethodReplicaRepository assemblyMethodReplicaRepository,
        ICapabilityReplicaRepository capabilityReplicaRepository)
    {
        _repository = repository;
        _codeGenerator = codeGenerator;
        _capabilityDetailRepository = capabilityDetailRepository;
        _unitOfWork = unitOfWork;
        _topicReplicaRepository = topicReplicaRepository;
        _materialReplicaRepository = materialReplicaRepository;
        _assemblyMethodReplicaRepository = assemblyMethodReplicaRepository;
        _capabilityReplicaRepository = capabilityReplicaRepository;
    }

    public async Task<ResultT<Guid>> Handle(
        CreateCustomDesignRequirementCommand request,
        CancellationToken cancellationToken)
    {

        // Validate Topic exists
        var topic = await _topicReplicaRepository.GetByIdAsync(request.TopicId, cancellationToken);
        if (topic is null)
            return Result.Failure<Guid>(CustomDesignRequirementError.TopicNotFound(request.TopicId));

        // Validate Material exists
        var material = await _materialReplicaRepository.GetByIdAsync(request.MaterialId, cancellationToken);
        if (material is null)
            return Result.Failure<Guid>(CustomDesignRequirementError.MaterialNotFound(request.MaterialId));

        // Validate AssemblyMethod exists
        var assemblyMethod = await _assemblyMethodReplicaRepository.GetByIdAsync(request.AssemblyMethodId, cancellationToken);
        if (assemblyMethod is null)
            return Result.Failure<Guid>(CustomDesignRequirementError.AssemblyMethodNotFound(request.AssemblyMethodId));

        // Validate capabilities
        
        if (request.CapabilityIds == null || request.CapabilityIds.ToList().Count == 0)
            return Result.Failure<Guid>(CustomDesignRequirementError.NoCapabilitiesSpecified());
        var capabilityIdsList = request.CapabilityIds.ToList();

        foreach (var capabilityId in capabilityIdsList)
        {
            var capability = await _capabilityReplicaRepository.GetByIdAsync(capabilityId, cancellationToken);
            if (capability is null)
                return Result.Failure<Guid>(CustomDesignRequirementError.CapabilityNotFound(capabilityId));
        }

        // Parse difficulty
        if (!Enum.TryParse<DifficultyLevel>(request.Difficulty, true, out var difficulty))
            return Result.Failure<Guid>(CustomDesignRequirementError.InvalidDifficulty(request.Difficulty));

        // Check for duplicate requirement
        var isDuplicate = await _repository.ExistsDuplicateAsync(
            request.TopicId,
            request.MaterialId,
            request.AssemblyMethodId,
            difficulty,
            capabilityIdsList,
            cancellationToken);

        if (isDuplicate)
            return Result.Failure<Guid>(CustomDesignRequirementError.DuplicateRequirement());

        // Generate code
        var code = await _codeGenerator.GenerateCodeAsync(cancellationToken);

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Create requirement
            var requirementResult = CustomDesignRequirement.Create(
                Guid.NewGuid(),
                code,
                request.TopicId,
                request.MaterialId,
                request.AssemblyMethodId,
                difficulty,
                request.MinPartQuantity,
                request.MaxPartQuantity,
                false,
                DateTime.UtcNow,
                DateTime.UtcNow);

            if (requirementResult.IsFailure)
                return Result.Failure<Guid>(requirementResult.Error);

            await _repository.AddAsync(requirementResult.Value, cancellationToken);

            // Add capability details
            foreach (var capabilityId in capabilityIdsList)
            {
                var detail = RequirementCapabilityDetail.Create(
                    Guid.NewGuid(),
                    requirementResult.Value.Id.Value,
                    capabilityId);
                await _capabilityDetailRepository.AddAsync(detail, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(requirementResult.Value.Id.Value);
        }, cancellationToken);
    }
}
