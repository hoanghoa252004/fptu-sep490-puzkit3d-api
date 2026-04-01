using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Authorization;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Application.User;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.Partners.Queries.GetAllPartners;

internal sealed class GetAllPartnersQueryHandler
    : IQueryHandler<GetAllPartnersQuery, PagedResult<object>>
{
    private readonly IPartnerRepository _partnerRepository;
    private readonly ICurrentUser _currentUser;

    public GetAllPartnersQueryHandler(
        IPartnerRepository partnerRepository,
        ICurrentUser currentUser)
    {
        _partnerRepository = partnerRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetAllPartnersQuery request,
        CancellationToken cancellationToken)
    {
        // Check if user is Staff or Manager
        var isStaffOrManager = _currentUser.IsAuthenticated &&
            (_currentUser.IsInRole(Roles.Staff) || _currentUser.IsInRole(Roles.BusinessManager));

        // Get all partners
        var allPartners = await _partnerRepository.GetAllAsync(isStaffOrManager, request.SearchTerm, request.Ascending, cancellationToken);

        // Apply pagination
        var totalCount = allPartners.Count();
        var partners = allPartners
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Build response DTOs
        IReadOnlyList<object> partnerDtos;
        if (isStaffOrManager)
        {
            partnerDtos = partners
                .Select(p => (object)new GetAllPartnersResponseDto(
                    p.Id.Value,
                    p.ImportServiceConfigId.Value,
                    p.Name,
                    p.Description,
                    p.ContactEmail,
                    p.ContactPhone,
                    p.Address,
                    p.Slug,
                    p.IsActive,
                    p.CreatedAt,
                    p.UpdatedAt))
                .ToList();
        }
        else
        {
            partnerDtos = partners.Select(p => (object)new GetAllPartnersPublicResponseDto(
                    p.Id.Value,
                    p.ImportServiceConfigId.Value,
                    p.Name,
                    p.Description,
                    p.ContactEmail,
                    p.ContactPhone,
                    p.Address,
                    p.Slug))
                .ToList();
        }

        var pagedResult = new PagedResult<object>(
            partnerDtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
