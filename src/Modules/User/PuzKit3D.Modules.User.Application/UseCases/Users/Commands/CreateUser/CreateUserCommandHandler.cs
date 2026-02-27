using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Application.UseCases.Users.Commands.CreateUser;

internal sealed class CreateUserCommandHandler : ICommandTHandler<CreateUserCommand, string>
{
    private readonly IIdentityService _identityService;

    public CreateUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ResultT<string>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        // Create user with specified role
        var result = await _identityService.CreateUserWithRoleAsync(
            request.Email,
            request.Password,
            request.Role,
            request.FirstName,
            request.LastName,
            cancellationToken);

        return result;
    }
}
