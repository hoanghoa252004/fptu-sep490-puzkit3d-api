using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Application.UseCases.Authentication.Login;

public sealed class LoginCommandHandler : ICommandTHandler<LoginCommand, AuthenticationResult>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ResultT<AuthenticationResult>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _identityService.LoginAsync(
            request.Email,
            request.Password,
            cancellationToken);

        return result;
    }
}
