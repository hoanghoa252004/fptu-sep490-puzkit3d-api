using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.Shared;
using PuzKit3D.SharedKernel.Application.Authorization;
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
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // Get all materials with filtering and sorting from repository
        var allMaterials = await _materialRepository.GetAllAsync(
            isStaffOrManager,
            request.SearchTerm,
            request.Ascending,
            cancellationToken);

        // Apply pagination
        var totalCount = allMaterials.Count();
        var materials = allMaterials
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Build response DTOs
        var materialDtos = isStaffOrManager
            ? materials.Select(m => (object)new GetMaterialDetailedResponseDto(
                m.Id.Value,
                m.Name,
                m.Slug,
                m.Description,
                m.IsActive,
                m.CreatedAt,
                m.UpdatedAt)).ToList()
            : materials.Select(m => (object)new GetMaterialResponseDto(
                m.Id.Value,
                m.Name,
                m.Slug,
                m.Description)).ToList();
        var pagedResult = new PagedResult<object>(
            materialDtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
