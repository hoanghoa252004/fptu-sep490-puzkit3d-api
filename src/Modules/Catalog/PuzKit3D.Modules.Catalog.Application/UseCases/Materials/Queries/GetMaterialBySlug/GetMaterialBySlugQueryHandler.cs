using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Materials.Queries.GetMaterialBySlug;

internal sealed class GetMaterialBySlugQueryHandler : IQueryHandler<GetMaterialBySlugQuery, GetMaterialBySlugPublicResponseDto>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly ICurrentUser _currentUser;

    public GetMaterialBySlugQueryHandler(
        IMaterialRepository materialRepository,
        ICurrentUser currentUser)
    {
        _materialRepository = materialRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<GetMaterialBySlugPublicResponseDto>> Handle(
        GetMaterialBySlugQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated && 
            (_currentUser.IsInRole("Staff") || _currentUser.IsInRole("Business Manager"));

        // Get material by slug
        var material = await _materialRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (material is null)
        {
            return Result.Failure<GetMaterialBySlugPublicResponseDto>(
                MaterialError.NotFoundBySlug(request.Slug));
        }

        // For non-staff/manager users, only return active materials
        if (!isStaffOrManager && !material.IsActive)
        {
            return Result.Failure<GetMaterialBySlugPublicResponseDto>(
                MaterialError.NotFoundBySlug(request.Slug));
        }

        var response = new GetMaterialBySlugPublicResponseDto(
            Id: material.Id.Value,
            Name: material.Name,
            Slug: material.Slug,
            Description: material.Description);

        return Result.Success(response);
    }
}
