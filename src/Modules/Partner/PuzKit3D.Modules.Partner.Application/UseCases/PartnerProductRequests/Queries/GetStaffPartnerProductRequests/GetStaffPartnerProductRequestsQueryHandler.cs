using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetStaffPartnerProductRequests;

internal sealed class GetStaffPartnerProductRequestsQueryHandler
    : IQueryHandler<GetStaffPartnerProductRequestsQuery, PagedResult<object>>
{
    private readonly IPartnerProductRequestRepository _repository;

    public GetStaffPartnerProductRequestsQueryHandler(
        IPartnerProductRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetStaffPartnerProductRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var allRequests = await _repository.GetAllAsync(cancellationToken);
        // Staff can see: Pending, Approved, RejectedByStaff
        var allowedStatuses = new[] { 
            (int)PartnerProductRequestStatus.Pending, 
            (int)PartnerProductRequestStatus.Approved, 
            (int)PartnerProductRequestStatus.RejectedByStaff 
        };
        
        var query = allRequests
            .Where(r => allowedStatuses.Contains(r.Status))
            .AsQueryable();

        // Apply pagination
        var totalCount = query.Count();
        var requests = query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = requests
            .Select(r => (object)new GetStaffPartnerProductRequestsResponseDto(
                r.Id.Value,
                r.Code,
                r.CustomerId,
                r.PartnerId.Value,
                r.DesiredDeliveryDate,
                r.TotalRequestedQuantity,
                r.Note,
                r.Status,
                r.CreatedAt))
            .ToList();

        var pagedResult = new PagedResult<object>(
            dtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
