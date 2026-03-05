using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UnitOfWork;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Commands.CreateCapability;

internal sealed class CreateCapabilityCommandHandler : ICommandTHandler<CreateCapabilityCommand, Guid>
{
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly ICatalogUnitOfWork _unitOfWork;

    public CreateCapabilityCommandHandler(
        ICapabilityRepository capabilityRepository,
        ICatalogUnitOfWork unitOfWork)
    {
        _capabilityRepository = capabilityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultT<Guid>> Handle(CreateCapabilityCommand request, CancellationToken cancellationToken)
    {
        // Check if slug already exists
        var existingCapability = await _capabilityRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (existingCapability is not null)
        {
            return Result.Failure<Guid>(CapabilityError.DuplicateSlug(request.Slug));
        }

        // Execute in transaction
        return await _unitOfWork.ExecuteAsync(async () =>
        {
            // Create capability using factory method
            var capabilityResult = Capability.Create(
                request.Name,
                request.Slug,
                request.Description,
                request.IsActive);

            if (capabilityResult.IsFailure)
            {
                return Result.Failure<Guid>(capabilityResult.Error);
            }

            var capability = capabilityResult.Value;

            // Add to repository
            _capabilityRepository.Add(capability);

            return Result.Success(capability.Id.Value);
        }, cancellationToken);
    }
}
