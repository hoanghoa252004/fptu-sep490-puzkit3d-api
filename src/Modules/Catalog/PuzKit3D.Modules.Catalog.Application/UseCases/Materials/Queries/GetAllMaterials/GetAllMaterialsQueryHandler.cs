using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.GetAllMaterials;

internal sealed class GetAllMaterialsQueryHandler 
    : IQueryHandler<GetAllMaterialsQuery, PagedResult<object>>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly ICurrentUser _currentUser;

    public GetAllMaterialsQueryHandler(
        IMaterialRepository materialRepository,
        ICurrentUser currentUser)
    {
        _materialRepository = materialRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetAllMaterialsQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated && 
            (_currentUser.IsInRole("Staff") || _currentUser.IsInRole("Business Manager"));

        // Get all materials
        var allMaterials = await _materialRepository.GetAllAsync(cancellationToken);
        var query = allMaterials.AsQueryable();

        // For non-staff/manager users (anonymous or customer), only show active items
        if (!isStaffOrManager)
        {
            query = query.Where(m => m.IsActive);
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(m => 
                m.Name.ToLower().Contains(searchTerm) || 
                m.Slug.ToLower().Contains(searchTerm) ||
                (m.Description != null && m.Description.ToLower().Contains(searchTerm)));
        }

        // Apply IsActive filter (only for staff/manager)
        if (isStaffOrManager && request.IsActive.HasValue)
        {
            query = query.Where(m => m.IsActive == request.IsActive.Value);
        }

        // Get total count before paging
        var totalCount = query.Count();

        // Apply sorting (by name ascending by default)
        query = query.OrderBy(m => m.Name);

        // Apply paging and map to appropriate DTO based on user role
        IReadOnlyList<object> items;
        
        if (isStaffOrManager)
        {
            // Staff/Manager: Return full details with timestamps
            items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(m => new GetAllMaterialsResponseDto(
                    m.Id.Value,
                    m.Name,
                    m.Slug,
                    m.Description,
                    m.IsActive,
                    m.CreatedAt,
                    m.UpdatedAt) as object)
                .ToList();
        }
        else
        {
            // Anonymous/Customer: Return public details without timestamps
            items = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(m => new GetAllMaterialsPublicResponseDto(
                    m.Id.Value,
                    m.Name,
                    m.Slug,
                    m.Description) as object)
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

public sealed record GetAllMaterialsResponseDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public sealed record GetAllMaterialsPublicResponseDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description);
