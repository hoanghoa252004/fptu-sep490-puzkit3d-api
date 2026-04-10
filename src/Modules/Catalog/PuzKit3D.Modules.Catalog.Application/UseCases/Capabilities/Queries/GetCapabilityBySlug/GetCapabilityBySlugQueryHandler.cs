using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Queries.GetCapabilityBySlug;

internal sealed class GetCapabilityBySlugQueryHandler : IQueryHandler<GetCapabilityBySlugQuery, object>
{
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly ICurrentUser _currentUser;

    public GetCapabilityBySlugQueryHandler(
        ICapabilityRepository capabilityRepository,
        ICurrentUser currentUser)
    {
        _capabilityRepository = capabilityRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<object>> Handle(
        GetCapabilityBySlugQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // Get capability by slug
        var capability = await _capabilityRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (capability is null)
        {
            return Result.Failure<object>(
                CapabilityError.NotFoundBySlug(request.Slug));
        }

        // For non-staff/manager users, only return active capabilities
        if (!isStaffOrManager && !capability.IsActive)
        {
            return Result.Failure<object>(
                CapabilityError.NotFoundBySlug(request.Slug));
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
