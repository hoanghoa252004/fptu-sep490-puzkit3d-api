using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Queries.GetCapabilityById;

internal sealed class GetCapabilityByIdQueryHandler : IQueryHandler<GetCapabilityByIdQuery, GetCapabilityByIdResponseDto>
{
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly ICurrentUser _currentUser;

    public GetCapabilityByIdQueryHandler(
        ICapabilityRepository capabilityRepository,
        ICurrentUser currentUser)
    {
        _capabilityRepository = capabilityRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<GetCapabilityByIdResponseDto>> Handle(
        GetCapabilityByIdQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated && 
            (_currentUser.IsInRole("Staff") || _currentUser.IsInRole("Business Manager"));

        // Get capability by ID
        var capabilityId = CapabilityId.From(request.Id);
        var capability = await _capabilityRepository.GetByIdAsync(capabilityId, cancellationToken);

        if (capability is null)
        {
            return Result.Failure<GetCapabilityByIdResponseDto>(
                CapabilityError.NotFound(request.Id));
        }

        // For non-staff/manager users, only return active capabilities
        if (!isStaffOrManager && !capability.IsActive)
        {
            return Result.Failure<GetCapabilityByIdResponseDto>(
                CapabilityError.NotFound(request.Id));
        }

        var response = new GetCapabilityByIdResponseDto(
            Id: capability.Id.Value,
            Name: capability.Name,
            Slug: capability.Slug,
            Description: capability.Description,
            IsActive: capability.IsActive,
            CreatedAt: capability.CreatedAt,
            UpdatedAt: capability.UpdatedAt);

        return Result.Success(response);
    }
}
