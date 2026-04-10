using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.Modules.Catalog.Domain.Entities.TopicMaterialCapabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.TopicMaterialCapabilities.Commands.CreateTopicMaterialCapability;

internal sealed class CreateTopicMaterialCapabilityCommandHandler
    : ICommandTHandler<CreateTopicMaterialCapabilityCommand, Guid>
{
    private readonly ITopicMaterialCapabilityRepository _repository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public CreateTopicMaterialCapabilityCommandHandler(
        ITopicMaterialCapabilityRepository repository,
        ICatalogUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(
        CreateTopicMaterialCapabilityCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsAsync(
            TopicId.From(request.TopicId),
            MaterialId.From(request.MaterialId),
            CapabilityId.From(request.CapabilityId),
            cancellationToken);

        if (exists)
        {
            return Result.Failure<Guid>(TopicMaterialCapabilitiesError.CombinationAlreadyExists);
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Create TopicMaterialCapability using factory method
            var item = TopicMaterialCapability.Create(
                TopicId.From(request.TopicId),
                MaterialId.From(request.MaterialId),
                CapabilityId.From(request.CapabilityId),
                request.IsActive);

            if (item.IsFailure)
            {
                return Result.Failure<Guid>(item.Error);
            }

            // Add to repository
            _repository.Add(item.Value);

            return Result.Success(item.Value.Id.Value);
        }, cancellationToken);
    }
}
