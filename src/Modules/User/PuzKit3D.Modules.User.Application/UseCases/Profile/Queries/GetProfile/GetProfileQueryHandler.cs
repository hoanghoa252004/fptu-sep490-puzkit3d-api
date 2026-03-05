using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Message.Query;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Application.UseCases.Profile.Queries.GetProfile;

internal sealed class GetProfileQueryHandler : IQueryHandler<GetProfileQuery, object>
{
    private readonly IIdentityService _identityService;

    public GetProfileQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ResultT<object>> Handle(
        GetProfileQuery request,
        CancellationToken cancellationToken)
    {
        return await _identityService.GetProfileAsync(
            request.UserId,
            cancellationToken);
    }
}
