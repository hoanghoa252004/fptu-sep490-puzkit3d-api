using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Queries.Shared;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Queries.GetAllCapabilities;

internal sealed class GetAllCapabilitiesQueryHandler 
    : IQueryHandler<GetAllCapabilitiesQuery, PagedResult<object>>
{
    private readonly ICapabilityRepository _capabilityRepository;
    private readonly ICurrentUser _currentUser;

    public GetAllCapabilitiesQueryHandler(
        ICapabilityRepository capabilityRepository,
        ICurrentUser currentUser)
    {
        _capabilityRepository = capabilityRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetAllCapabilitiesQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // Get all capabilities with filtering and sorting from repository
        var allCapabilities = await _capabilityRepository.GetAllAsync(
            isStaffOrManager,
            request.SearchTerm,
            request.Ascending,
            cancellationToken);

        // Apply pagination
        var totalCount = allCapabilities.Count();
        var capabilities = allCapabilities
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Build response DTOs
        var capabilityDtos = isStaffOrManager
            ? capabilities.Select(c => (object)new GetCapabilityDetailedResponseDto(
                c.Id.Value,
                c.Name,
                c.Slug,
                c.Description,
                c.IsActive,
                c.CreatedAt,
                c.UpdatedAt)).ToList()
            : capabilities.Select(c => (object)new GetCapabilityResponseDto(
                c.Id.Value,
                c.Name,
                c.Slug,
                c.Description)).ToList();

        var pagedResult = new PagedResult<object>(
            capabilityDtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
