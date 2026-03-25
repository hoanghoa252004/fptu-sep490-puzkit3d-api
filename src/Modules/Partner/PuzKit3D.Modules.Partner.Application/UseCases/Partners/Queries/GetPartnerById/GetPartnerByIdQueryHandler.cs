using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Queries.GetPartnerById;

internal sealed class GetPartnerByIdQueryHandler
    : IQueryHandler<GetPartnerByIdQuery, object>
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly ICurrentUser _currentUser;

    public GetPartnerByIdQueryHandler(
        IPartnerRepository partnerRepository,
        ICurrentUser currentUser)
    {
        _partnerRepository = partnerRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<object>> Handle(
        GetPartnerByIdQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        var partner = await _partnerRepository.GetByIdAsync(
            PartnerId.From(request.Id),
            cancellationToken);

        if (partner is null)
        {
            return Result.Failure<object>(PartnerError.NotFound(request.Id));
        }

        // For non-staff/manager users, only allow viewing active partners
        if (!isStaffOrManager && !partner.IsActive)
        {
            return Result.Failure<object>(PartnerError.NotFound(request.Id));
        }

        // Build response DTO
        object responseDto;
        if (isStaffOrManager)
        {
            responseDto = new GetPartnerByIdResponseDto(
                partner.Id.Value,
                partner.ImportServiceConfigId.Value,
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
            responseDto = new GetPartnerByIdPublicResponseDto(
                partner.Id.Value,
                partner.ImportServiceConfigId.Value,
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
