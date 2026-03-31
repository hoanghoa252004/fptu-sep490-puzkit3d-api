using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetManagerPartnerProductRequests;

internal sealed class GetManagerPartnerProductRequestsQueryHandler
    : IQueryHandler<GetManagerPartnerProductRequestsQuery, PagedResult<object>>
{
    private readonly IPartnerProductRequestRepository _repository;

    public GetManagerPartnerProductRequestsQueryHandler(
        IPartnerProductRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetManagerPartnerProductRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var allRequests = await _repository.GetAllAsync(cancellationToken);
        // Manager can see: Approved, Quoted, Rejected, Cancelled
        var allowedStatuses = new[] { 
            (int)PartnerProductRequestStatus.Approved, 
            (int)PartnerProductRequestStatus.Quoted,
            (int)PartnerProductRequestStatus.RejectedByStaff,
            (int)PartnerProductRequestStatus.Cancelled
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
            .Select(r => (object)new GetManagerPartnerProductRequestsResponseDto(
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
