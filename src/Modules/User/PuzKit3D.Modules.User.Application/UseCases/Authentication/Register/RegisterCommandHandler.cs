using MediatR;
using PuzKit3D.Contract.User;
using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Authentication.Dtos;
using PuzKit3D.SharedKernel.Application.Data;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Identity;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Errors;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Application.UseCases.Authentication.Register;

public sealed class RegisterCommandHandler : ICommandTHandler<RegisterCommand, string>
{
    private readonly IIdentityService _identityService;
    private readonly IIdentityUnitOfWork _uow;
    private readonly IEventBus _eventBus;

    public RegisterCommandHandler(IIdentityService identityService, IEventBus eventBus, IIdentityUnitOfWork uow)
    {
        _identityService = identityService;
        _eventBus = eventBus;
        _uow = uow;
    }

    public async Task<ResultT<string>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        return await _uow.ExecuteAsync(async () => 
        {
            var normalizeEmail = request.Email.ToLowerInvariant();
            var checkEmailExist = await _identityService.CheckEmailExist(normalizeEmail, cancellationToken);

            if (checkEmailExist.IsSuccess) // đã đăng kí rồi
            {
                var checkEmailConfirmed = await _identityService.CheckEmailConfirmed(normalizeEmail, cancellationToken); 

                if (checkEmailConfirmed.IsFailure) // chưa xác nhận email
                {
                    // Gửi lại link xác nhận
                    var getUserIdResult = await _identityService.GetUserIdByEmail(normalizeEmail, cancellationToken);
                    var getTokenResult = await _identityService.GenerateConfirmEmailToken(normalizeEmail, cancellationToken);

                    await _eventBus.PublishAsync(
                    new UserRegisteredIntegrationEvent(
                        Guid.NewGuid(),
                        DateTime.UtcNow,
                        normalizeEmail,
                        getUserIdResult.Value,
                        getTokenResult.Value
                    ), cancellationToken);

                    return Result.Failure<string>(checkEmailConfirmed.Error);

                }
                else // đã xác nhận email rồi
                    return Result.Failure<string>(Error.Conflict("Authentication.EmailAlreadyExists", $"User with email {normalizeEmail} already exists"));
            }
            else // chưa đăng kí ( chưa có user nào với email này )
            {
                var result = await _identityService.RegisterAsync(
                normalizeEmail,
                request.Password,
                request.FirstName,
                request.LastName,
                cancellationToken);

                if (result.IsFailure)
                    return Result.Failure<string>(result.Error);
                
                await _eventBus.PublishAsync(
                new UserRegisteredIntegrationEvent(
                    Guid.NewGuid(),
                    DateTime.UtcNow,
                    normalizeEmail,
                    result.Value.UserId,
                    result.Value.Token
                ), cancellationToken);

                return Result.Success($"User registered successfully with email {normalizeEmail}, please confirm email ");
            }
        });
    }
}
