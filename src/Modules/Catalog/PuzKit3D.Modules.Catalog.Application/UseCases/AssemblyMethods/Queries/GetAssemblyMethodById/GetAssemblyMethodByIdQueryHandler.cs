using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.Shared;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.AssemblyMethods.Queries.GetAssemblyMethodById;

internal sealed class GetAssemblyMethodByIdQueryHandler
    : IQueryHandler<GetAssemblyMethodByIdQuery, object>
{
    private readonly IAssemblyMethodRepository _assemblyMethodRepository;
    private readonly ICurrentUser _currentUser;

    public GetAssemblyMethodByIdQueryHandler(
        IAssemblyMethodRepository assemblyMethodRepository,
        ICurrentUser currentUser)
    {
        _assemblyMethodRepository = assemblyMethodRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<object>> Handle(
        GetAssemblyMethodByIdQuery request,
        CancellationToken cancellationToken)
    {

        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // Get assembly method by ID
        var assemblyMethodId = AssemblyMethodId.From(request.Id);
        var assemblyMethod = await _assemblyMethodRepository.GetByIdAsync(assemblyMethodId, cancellationToken);

        if (assemblyMethod is null)
        {
            return Result.Failure<object>(
                AssemblyMethodError.NotFound(request.Id));
        }

        // For non-staff/manager users, only return active assembly methods
        if (!isStaffOrManager && !assemblyMethod.IsActive)
        {
            return Result.Failure<object>(
                AssemblyMethodError.NotFound(request.Id));
        }

        // Map to DTO
        var response = isStaffOrManager
            ? (object)new GetAssemblyMethodDetailedResponseDto(
                assemblyMethod.Id.Value,
                assemblyMethod.Name,
                assemblyMethod.Slug,
                assemblyMethod.Description,
                assemblyMethod.IsActive,
                assemblyMethod.CreatedAt,
                assemblyMethod.UpdatedAt)
            : (object)new GetAssemblyMethodByIdPublicResponseDto(
                assemblyMethod.Id.Value,
                assemblyMethod.Name,
                assemblyMethod.Slug,
                assemblyMethod.Description);

        return Result.Success(response);
    }
}

