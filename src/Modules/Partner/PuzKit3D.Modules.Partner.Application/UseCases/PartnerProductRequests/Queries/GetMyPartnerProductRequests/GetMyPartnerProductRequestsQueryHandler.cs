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
        var allRequests = await _repository.GetAllAsync(
            request.Status, 
            request.SearchTerm, 
            request.Ascending, 
            cancellationToken);

        var totalCount = allRequests.Count();
        var dtos = allRequests
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
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
