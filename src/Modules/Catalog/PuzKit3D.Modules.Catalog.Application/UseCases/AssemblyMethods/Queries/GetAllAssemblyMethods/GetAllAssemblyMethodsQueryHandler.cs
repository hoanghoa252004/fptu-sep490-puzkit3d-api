using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.Shared;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAllAssemblyMethods;

internal sealed class GetAllAssemblyMethodsQueryHandler 
    : IQueryHandler<GetAllAssemblyMethodsQuery, PagedResult<object>>
{
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;
    private readonly ICurrentUser _currentUser;

    public GetAllAssemblyMethodsQueryHandler(
        IAssemblyMethodRepository assemblyMethodRepository,
        ICurrentUser currentUser)
    {
        _assemblyMethodRepository = assemblyMethodRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetAllAssemblyMethodsQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // Get all assembly methods with filtering and sorting from repository
        var allMethods = await _assemblyMethodRepository.GetAllAsync(
            isStaffOrManager,
            request.SearchTerm,
            request.Ascending,
            cancellationToken);

        // Apply pagination
        var totalCount = allMethods.Count();
        var methods = allMethods
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Build response DTOs
        var methodDtos = isStaffOrManager
            ? methods.Select(a => (object)new GetAssemblyMethodDetailedResponseDto(
                a.Id.Value,
                a.Name,
                a.Slug,
                a.Description,
                a.FactorPercentage,
                a.IsActive,
                a.CreatedAt,
                a.UpdatedAt)).ToList()
            : methods.Select(a => (object)new GetAssemblyMethodResponseDto(
                a.Id.Value,
                a.Name,
                a.Slug,
                a.Description,
                a.FactorPercentage)).ToList();

        var pagedResult = new PagedResult<object>(
            methodDtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
