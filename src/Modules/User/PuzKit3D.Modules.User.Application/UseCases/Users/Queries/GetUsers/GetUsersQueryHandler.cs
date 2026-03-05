using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Queries.GetUsers;

internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, object>
{
    private readonly IIdentityService _identityService;

    public GetUsersQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ResultT<object>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        return await _identityService.GetUsersAsync(
            request.PageNumber,
            request.PageSize,
            request.SearchTerm,
            cancellationToken);
    }
}
