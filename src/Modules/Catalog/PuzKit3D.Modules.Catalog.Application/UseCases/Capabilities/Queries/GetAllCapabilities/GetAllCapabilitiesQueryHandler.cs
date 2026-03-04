using PuzKit3D.Modules.Catalog.Application.Repositories;
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
            (_currentUser.IsInRole("Staff") || _currentUser.IsInRole("Business Manager"));

        // Get all capabilities
        var allCapabilities = await _capabilityRepository.GetAllAsync(cancellationToken);
        var query = allCapabilities.AsQueryable();

        // For non-staff/manager users (anonymous or customer), only show active items
        if (!isStaffOrManager)
        {
            query = query.Where(c => c.IsActive);
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(c => 
                c.Name.ToLower().Contains(searchTerm) || 
                c.Slug.ToLower().Contains(searchTerm) ||
                (c.Description != null && c.Description.ToLower().Contains(searchTerm)));
        }

        // Apply IsActive filter (only for staff/manager)
        if (isStaffOrManager && request.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == request.IsActive.Value);
        }

        // Get total count before paging
        var totalCount = query.Count();

        // Apply sorting (by name ascending by default)
        query = query.OrderBy(c => c.Name);

        // Apply paging and map to appropriate DTO based on user role
        IReadOnlyList<object> items;
        
        if (isStaffOrManager)
        {
            // Staff/Manager: Return full details with timestamps
            items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new GetAllCapabilitiesResponseDto(
                    c.Id.Value,
                    c.Name,
                    c.Slug,
                    c.Description,
                    c.IsActive,
                    c.CreatedAt,
                    c.UpdatedAt) as object)
                .ToList();
        }
        else
        {
            // Anonymous/Customer: Return public details without timestamps
            items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new GetAllCapabilitiesPublicResponseDto(
                    c.Id.Value,
                    c.Name,
                    c.Slug,
                    c.Description) as object)
                .ToList();
        }

        var pagedResult = PagedResult<object>.Create(
            items,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}

public sealed record GetAllCapabilitiesResponseDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public sealed record GetAllCapabilitiesPublicResponseDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description);
