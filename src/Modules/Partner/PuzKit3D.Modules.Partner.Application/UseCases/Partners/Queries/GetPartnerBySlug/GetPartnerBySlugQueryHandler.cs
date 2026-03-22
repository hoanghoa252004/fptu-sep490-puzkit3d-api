using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners;

namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Queries.GetPartnerBySlug;

internal sealed class GetPartnerBySlugQueryHandler
    : IQueryHandler<GetPartnerBySlugQuery, object>
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly ICurrentUser _currentUser;

    public GetPartnerBySlugQueryHandler(
        IPartnerRepository partnerRepository,
        ICurrentUser currentUser)
    {
        _partnerRepository = partnerRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<object>> Handle(
        GetPartnerBySlugQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        var partner = await _partnerRepository.GetBySlugAsync(request.Slug, cancellationToken);

        if (partner is null)
        {
            return Result.Failure<object>(
                PartnerError.NotFoundBySlug(request.Slug));
        }

        // For non-staff/manager users, only allow viewing active partners
        if (!isStaffOrManager && !partner.IsActive)
        {
            return Result.Failure<object>(
                Error.NotFound(
                    "Partner.NotActive",
                    $"Partner with slug '{request.Slug}' is not available."));
        }

        // Build response DTO
        object responseDto;
        if (isStaffOrManager)
        {
            responseDto = new GetPartnerBySlugResponseDto(
                partner.Id.Value,
                partner.Name,
                partner.Description,
                partner.ContactEmail,
                partner.ContactPhone,
                partner.Address,
                partner.Slug,
                partner.IsActive,
                partner.CreatedAt,
                partner.UpdatedAt);
        }
        else
        {
            responseDto = new GetPartnerBySlugPublicResponseDto(
                partner.Id.Value,
                partner.Name,
                partner.Description,
                partner.ContactEmail,
                partner.ContactPhone,
                partner.Address,
                partner.Slug);
        }

        return Result.Success(responseDto);
    }
}
