using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Queries.GetCapabilityById;

internal sealed class GetCapabilityByIdQueryHandler : IQueryHandler<GetCapabilityByIdQuery, object>
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

    public async Task<ResultT<object>> Handle(
        GetCapabilityByIdQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated && 
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // Get capability by ID
        var capabilityId = CapabilityId.From(request.Id);
        var capability = await _capabilityRepository.GetByIdAsync(capabilityId, cancellationToken);

        if (capability is null)
        {
            return Result.Failure<object>(
                CapabilityError.NotFound(request.Id));
        }

        // For non-staff/manager users, only return active capabilities
        if (!isStaffOrManager && !capability.IsActive)
        {
            return Result.Failure<object>(
                CapabilityError.NotFound(request.Id));
        }

        // Build response DTO based on user role
        object response = isStaffOrManager
            ? new GetCapabilityDetailedResponseDto(
                capability.Id.Value,
                capability.Name,
                capability.Slug,
                capability.Description,
                capability.IsActive,
                capability.CreatedAt,
                capability.UpdatedAt)
            : new GetCapabilityResponseDto(
                capability.Id.Value,
                capability.Name,
                capability.Slug,
                capability.Description);
        return Result.Success(response);
    }
}
