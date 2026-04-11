using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Commands.DeleteCapability;

internal sealed class DeleteCapabilityCommandHandler : ICommandHandler<DeleteCapabilityCommand>
{
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;
    private readonly ICapabilityMaterialAssemblyRepository _capabilityMaterialAssemblyRepository;
    private readonly ITopicMaterialCapabilityRepository _topicMaterialCapabilityRepository;

    public DeleteCapabilityCommandHandler(
        ICapabilityRepository capabilityRepository,
        ICatalogUnitOfWork unitOfWork,
        ICapabilityMaterialAssemblyRepository capabilityMaterialAssemblyRepository,
        ITopicMaterialCapabilityRepository topicMaterialCapabilityRepository)
    {
        _capabilityRepository = capabilityRepository;
        _unitOfWork = unitOfWork;
        _capabilityMaterialAssemblyRepository = capabilityMaterialAssemblyRepository;
        _topicMaterialCapabilityRepository = topicMaterialCapabilityRepository;
    }

    public async Task<Result> Handle(DeleteCapabilityCommand request, CancellationToken cancellationToken)
    {
        // Get capability by ID
        var capabilityId = CapabilityId.From(request.Id);
        var capability = await _capabilityRepository.GetByIdAsync(capabilityId, cancellationToken);

        if (capability is null)
        {
            return Result.Failure(CapabilityError.NotFound(request.Id));
        }

        var existingCapabilityMaterialAssemblies = await _capabilityMaterialAssemblyRepository
            .GetCapabilityMaterialAssembliesByCapabilityIdAsync(capabilityId, cancellationToken);

        if (existingCapabilityMaterialAssemblies is not null && existingCapabilityMaterialAssemblies.Any())
        {
            return Result.Failure(CapabilityError.InUse(request.Id));
        }

        var existingTopicMaterialCapabilities = await _topicMaterialCapabilityRepository
            .GetTopicMaterialCapabilitiesByCapabilityIdAsync(capabilityId, cancellationToken);

        if (existingTopicMaterialCapabilities is not null && existingTopicMaterialCapabilities.Any())
        {
            return Result.Failure(CapabilityError.InUse(request.Id));
        }

        // Execute in transaction
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            capability.Delete();
            // Delete from repository
            _capabilityRepository.Delete(capability);

            return Result.Success();
        }, cancellationToken);
    }
}
