using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAssemblyMethodBySlug;

internal sealed class GetAssemblyMethodBySlugQueryHandler : IQueryHandler<GetAssemblyMethodBySlugQuery, object>
{
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;
    private readonly ICurrentUser _currentUser;

    public GetAssemblyMethodBySlugQueryHandler(IAssemblyMethodRepository assemblyMethodRepository, ICurrentUser currentUser)
    {
        _assemblyMethodRepository = assemblyMethodRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<object>> Handle(
        GetAssemblyMethodBySlugQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // Get assembly method by slug
        var assemblyMethod = await _assemblyMethodRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (assemblyMethod is null)
        {
            return Result.Failure<object>(
                AssemblyMethodError.NotFoundBySlug(request.Slug));
        }

        // For non-staff/manager users, only return active assembly methods
        if (!isStaffOrManager && !assemblyMethod.IsActive)
        {
            return Result.Failure<object>(
                AssemblyMethodError.NotFoundBySlug(request.Slug));
        }

        // Map to DTO based on user role
        var response = isStaffOrManager
            ? (object)new GetAssemblyMethodBySlugResponseDto(
                assemblyMethod.Id.Value,
                assemblyMethod.Name,
                assemblyMethod.Slug,
                assemblyMethod.Description,
                assemblyMethod.IsActive,
                assemblyMethod.CreatedAt,
                assemblyMethod.UpdatedAt)
            : new GetAssemblyMethodBySlugPublicResponseDto(
                assemblyMethod.Id.Value,
                assemblyMethod.Name,
                assemblyMethod.Slug,
                assemblyMethod.Description
            );

        return Result.Success(response);
    }
}


