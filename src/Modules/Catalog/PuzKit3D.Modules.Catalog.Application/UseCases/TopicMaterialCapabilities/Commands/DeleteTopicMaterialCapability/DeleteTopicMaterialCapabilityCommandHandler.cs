using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.TopicMaterialCapabilities;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Commands.DeleteTopicMaterialCapability;

internal sealed class DeleteTopicMaterialCapabilityCommandHandler : ICommandHandler<DeleteTopicMaterialCapabilityCommand>
{
    private readonly ITopicMaterialCapabilityRepository _repository;
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public DeleteTopicMaterialCapabilityCommandHandler(
        ITopicMaterialCapabilityRepository repository,
        ICapabilityRepository capabilityRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _repository = repository;
        _capabilityRepository = capabilityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteTopicMaterialCapabilityCommand request,
        CancellationToken cancellationToken)
    {
        var capabilityId = CapabilityId.From(request.CapabilityId);
        var capability = await _capabilityRepository.GetByIdAsync(capabilityId, cancellationToken);

        if (capability is null)
            return Result.Failure(CapabilityError.NotFound(request.CapabilityId));

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var id = TopicMaterialCapabilityId.From(request.TopicMaterialCapabilityId);
            var item = await _repository.GetByIdAsync(id, cancellationToken);

            if (item is null)
                return Result.Failure(TopicMaterialCapabilitiesError.NotFound(request.TopicMaterialCapabilityId));

            if (item.CapabilityId != capabilityId)
                return Result.Failure(TopicMaterialCapabilitiesError.NotFound(request.TopicMaterialCapabilityId));

            // Delete the item
            _repository.Delete(item);

            return Result.Success();
        }, cancellationToken);
    }
}
