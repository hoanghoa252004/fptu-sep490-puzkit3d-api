using MediatR;
using PuzKit3D.Contract.User;
using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Application.UseCases.Authentication.Register;

public sealed class RegisterCommandHandler : ICommandTHandler<RegisterCommand, string>
{
    private readonly IIdentityService _identityService;
    private readonly IEventBus _eventBus;

    public RegisterCommandHandler(IIdentityService identityService, IEventBus eventBus)
    {
        _identityService = identityService;
        _eventBus = eventBus;
    }

    public async Task<ResultT<string>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _identityService.RegisterAsync(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            cancellationToken);

        if(result.IsSuccess)
        {
            await _eventBus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                Guid.NewGuid(),
                DateTime.UtcNow,
                request.Email,
                result.Value
            ), cancellationToken);
        }

        return Result.Success($"User registered successfully with email {request.Email}, please confirm email ");
    }
}
