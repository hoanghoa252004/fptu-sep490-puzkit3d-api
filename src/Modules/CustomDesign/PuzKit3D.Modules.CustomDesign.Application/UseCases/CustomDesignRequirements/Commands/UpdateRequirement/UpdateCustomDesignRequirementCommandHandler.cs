using PuzKit3D.Modules.CustomDesign.Application.Repositories;
using PuzKit3D.Modules.CustomDesign.Application.UnitOfWork;
using PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Queries.GetRequirementById;
using PuzKit3D.Modules.CustomDesign.Domain.Entities;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.RequirementCapabilityDetails;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequirements.Commands.UpdateRequirement;

internal sealed class UpdateCustomDesignRequirementCommandHandler : ICommandHandler<UpdateCustomDesignRequirementCommand>
{
    private readonly ICustomDesignRequirementRepository _repository;
    private readonly IRequirementCapabilityDetailRepository _capabilityDetailRepository;
    private readonly ICustomDesignUnitOfWork _unitOfWork;
    private readonly ITopicReplicaRepository _topicReplicaRepository;
    private readonly IMaterialReplicaRepository _materialReplicaRepository;
    private readonly IAssemblyMethodReplicaRepository _assemblyMethodReplicaRepository;
    private readonly ICapabilityReplicaRepository _capabilityReplicaRepository;

    public UpdateCustomDesignRequirementCommandHandler(
        ICustomDesignRequirementRepository repository,
        IRequirementCapabilityDetailRepository capabilityDetailRepository,
        ICustomDesignUnitOfWork unitOfWork,
        ITopicReplicaRepository topicReplicaRepository,
        IMaterialReplicaRepository materialReplicaRepository,
        IAssemblyMethodReplicaRepository assemblyMethodReplicaRepository,
        ICapabilityReplicaRepository capabilityReplicaRepository)
    {
        _repository = repository;
        _capabilityDetailRepository = capabilityDetailRepository;
        _unitOfWork = unitOfWork;
        _topicReplicaRepository = topicReplicaRepository;
        _materialReplicaRepository = materialReplicaRepository;
        _assemblyMethodReplicaRepository = assemblyMethodReplicaRepository;
        _capabilityReplicaRepository = capabilityReplicaRepository;
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

        // Validate capabilities if provided
        IEnumerable<Guid>? capabilityIdsList = null;
        if (request.CapabilityIds != null)
        {
            capabilityIdsList = request.CapabilityIds.ToList();
            if (capabilityIdsList.Any())
            {
                foreach (var capabilityId in capabilityIdsList)
                {
                    var capability = await _capabilityReplicaRepository.GetByIdAsync(capabilityId, cancellationToken);
                    if (capability is null)
                        return Result.Failure(CustomDesignRequirementError.CapabilityNotFound(capabilityId));
                }
            }
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Check if capabilities are being updated
            bool capabilitiesChanged = false;
            if (capabilityIdsList != null && capabilityIdsList.Any())
            {
                var existingCapabilities = await _capabilityDetailRepository
                    .GetByRequirementIdAsync(requirementResult.Value.Id, cancellationToken);

                var existingCapabilityIds = existingCapabilities
                    .Select(c => c.CapabilityId)
                    .OrderBy(c => c)
                    .ToList();

                var newCapabilityIds = capabilityIdsList
                    .OrderBy(c => c)
                    .ToList();

                // Check if capabilities have actually changed
                if (existingCapabilityIds.Count != newCapabilityIds.Count ||
                    !existingCapabilityIds.SequenceEqual(newCapabilityIds))
                {
                    capabilitiesChanged = true;
                }
            }

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

            // Check if there were any changes (either requirement properties or capabilities)
            bool hasRequirementChanges = updateResult.IsSuccess;
            if (!hasRequirementChanges && !capabilitiesChanged)
                return Result.Failure(CustomDesignRequirementError.NothingToUpdate());

            // If requirement properties changed, check for errors
            if (updateResult.IsFailure && capabilitiesChanged == false)
                return Result.Failure(updateResult.Error);

            // Only update repository if requirement properties changed
            if (hasRequirementChanges)
                _repository.Update(requirementResult.Value);

            // Update capabilities if they changed
            if (capabilitiesChanged)
            {
                var existingCapabilities = await _capabilityDetailRepository
                    .GetByRequirementIdAsync(requirementResult.Value.Id, cancellationToken);

                // Delete all existing capabilities
                foreach (var capability in existingCapabilities)
                {
                    _capabilityDetailRepository.Delete(capability);
                }

                // Add new capabilities
                foreach (var capabilityId in capabilityIdsList!)
                {
                    var detail = RequirementCapabilityDetail.Create(
                        Guid.NewGuid(),
                        requirementResult.Value.Id.Value,
                        capabilityId);
                    await _capabilityDetailRepository.AddAsync(detail, cancellationToken);
                }
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }, cancellationToken);
    }
}

