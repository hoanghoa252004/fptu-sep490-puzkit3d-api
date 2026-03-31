using PuzKit3D.Modules.Partner.Application.Repositories;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Partner.Application.UseCases.PartnerProductRequests.Queries.GetMyPartnerProductRequests;

internal sealed class GetMyPartnerProductRequestsQueryHandler
    : IQueryHandler<GetMyPartnerProductRequestsQuery, PagedResult<object>>
{
    private readonly IPartnerProductRequestRepository _repository;

    public GetMyPartnerProductRequestsQueryHandler(
        IPartnerProductRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultT<PagedResult<object>>> Handle(
        GetMyPartnerProductRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var allRequests = await _repository.GetAllAsync(cancellationToken);
        var query = allRequests
            .Where(r => r.CustomerId == request.CustomerId)
            .AsQueryable();

        // Filter by status if provided
        if (request.Status.HasValue)
        {
            query = query.Where(r => r.Status.ToString() == request.Status.Value.ToString());
        }

        // Filter by created date range
        if (request.CreatedAtFrom.HasValue)
        {
            query = query.Where(r => r.CreatedAt >= request.CreatedAtFrom.Value);
        }

        if (request.CreatedAtTo.HasValue)
        {
            query = query.Where(r => r.CreatedAt <= request.CreatedAtTo.Value.AddDays(1));
        }

        // Apply pagination
        var totalCount = query.Count();
        var requests = query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = requests
            .Select(r => (object)new GetMyPartnerProductRequestsResponseDto(
                r.Id.Value,
                r.Code,
                r.PartnerId.Value,
                r.DesiredDeliveryDate,
                r.TotalRequestedQuantity,
                r.Note,
                r.Status,
                r.CreatedAt,
                r.UpdatedAt))
            .ToList();

        var pagedResult = new PagedResult<object>(
            dtos,
            request.PageNumber,
            request.PageSize,
            totalCount);

        return Result.Success(pagedResult);
    }
}
