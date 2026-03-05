using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Application.UseCases.Authentication.RefreshToken;

internal sealed class RefreshTokenCommandHandler : ICommandTHandler<RefreshTokenCommand, AuthenticationResult>
{
    private readonly IIdentityService _identityService;

    public RefreshTokenCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ResultT<AuthenticationResult>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        return await _identityService.RefreshTokenAsync(
            request.RefreshToken,
            cancellationToken);
    }
}
