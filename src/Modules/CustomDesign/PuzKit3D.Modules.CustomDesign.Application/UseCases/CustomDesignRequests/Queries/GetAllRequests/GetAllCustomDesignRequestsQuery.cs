using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Application.Pagination;

namespace PuzKit3D.Modules.CustomDesign.Application.UseCases.CustomDesignRequests.Queries.GetAllRequests;

public sealed record GetAllCustomDesignRequestsQuery(
    int PageNumber,
    int PageSize,
    string? Status) : IQuery<PagedResult<GetAllCustomDesignRequestsResponseDto>>;

