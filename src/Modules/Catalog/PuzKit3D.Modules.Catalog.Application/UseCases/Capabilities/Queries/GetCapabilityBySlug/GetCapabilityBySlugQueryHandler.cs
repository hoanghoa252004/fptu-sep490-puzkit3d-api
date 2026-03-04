using PuzKit3D.Modules.Catalog.Application.Repositories;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Catalog.Application.UseCases.Capabilities.Queries.GetCapabilityBySlug;

internal sealed class GetCapabilityBySlugQueryHandler : IQueryHandler<GetCapabilityBySlugQuery, GetCapabilityBySlugPublicResponseDto>
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

    public async Task<ResultT<GetCapabilityBySlugPublicResponseDto>> Handle(
        GetCapabilityBySlugQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated && 
            (_currentUser.IsInRole("Staff") || _currentUser.IsInRole("Business Manager"));

        // Get capability by slug
        var capability = await _capabilityRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (capability is null)
        {
            return Result.Failure<GetCapabilityBySlugPublicResponseDto>(
                CapabilityError.NotFoundBySlug(request.Slug));
        }

        // For non-staff/manager users, only return active capabilities
        if (!isStaffOrManager && !capability.IsActive)
        {
            return Result.Failure<GetCapabilityBySlugPublicResponseDto>(
                CapabilityError.NotFoundBySlug(request.Slug));
        }

        var response = new GetCapabilityBySlugPublicResponseDto(
            Id: capability.Id.Value,
            Name: capability.Name,
            Slug: capability.Slug,
            Description: capability.Description);

        return Result.Success(response);
    }
}
