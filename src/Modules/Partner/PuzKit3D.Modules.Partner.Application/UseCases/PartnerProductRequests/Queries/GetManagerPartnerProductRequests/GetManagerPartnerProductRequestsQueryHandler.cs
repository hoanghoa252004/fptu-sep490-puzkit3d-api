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
        

        // Manager can see: Approved, Quoted, Rejected, Cancelled
        var allowedStatuses = new HashSet<PartnerProductRequestStatus> { 
            PartnerProductRequestStatus.Approved, 
            PartnerProductRequestStatus.Quoted,
            PartnerProductRequestStatus.RejectedByCustomer,
            PartnerProductRequestStatus.CancelledByStaff,
            PartnerProductRequestStatus.CancelledByCustomer
        };

        var allRequests = await _repository.GetAllAsync(
            allowedStatuses, 
            request.SearchTerm, 
            cancellationToken);

        // Apply pagination
        var totalCount = allRequests.Count();

        var dtos = allRequests
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(r => (object)new GetManagerPartnerProductRequestsResponseDto(
                r.Id.Value,
                r.Code,
                r.CustomerId,
                r.PartnerId.Value,
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
