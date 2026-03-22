using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Commands.UpdateCapability;

internal sealed class UpdateCapabilityCommandHandler : ICommandHandler<UpdateCapabilityCommand>
{
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public UpdateCapabilityCommandHandler(
        ICapabilityRepository capabilityRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _capabilityRepository = capabilityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCapabilityCommand request, CancellationToken cancellationToken)
    {
        // Get capability by ID
        var capabilityId = CapabilityId.From(request.Id);
        var capability = await _capabilityRepository.GetByIdAsync(capabilityId, cancellationToken);

        if (capability is null)
        {
            return Result.Failure(CapabilityError.NotFound(request.Id));
        }

        // Check if slug is being changed to an existing one
        if (capability.Slug != request.Slug)
        {
            var existingCapability = await _capabilityRepository.GetBySlugAsync(request.Slug, cancellationToken);
            if (existingCapability is not null)
            {
                return Result.Failure(CapabilityError.DuplicateSlug(request.Slug));
            }
        }

        // Execute in transaction
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Update capability
            var updateResult = capability.Update(
                request.Name,
                request.Slug,
                request.Description,
                request.IsActive);

            if (updateResult.IsFailure)
            {
                return updateResult;
            }

            // Update in repository
            _capabilityRepository.Update(capability);

            return Result.Success();
        }, cancellationToken);
    }
}
