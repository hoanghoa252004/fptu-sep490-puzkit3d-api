using MediatR;
using PuzKit3D.Contract.User;
using PuzKit3D.SharedKernel.Application.Authentication;
using PuzKit3D.SharedKernel.Application.Data;
using PuzKit3D.SharedKernel.Application.Event;
using PuzKit3D.SharedKernel.Application.Identity;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.User.Application.UseCases.Authentication.ForgotPassword;

public sealed class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand>
{
    private readonly IIdentityService _identityService;
    private readonly IIdentityUnitOfWork _uow;
    private readonly IEventBus _eventBus;
    private const string ResetPasswordUrl = "http://localhost:3000/reset-password";

    public ForgotPasswordCommandHandler(
        IIdentityService identityService,
        IEventBus eventBus,
        IIdentityUnitOfWork uow)
    {
        _identityService = identityService;
        _eventBus = eventBus;
        _uow = uow;
    }

    public async Task<Result> Handle(
        ForgotPasswordCommand request,
        CancellationToken cancellationToken)
    {
        return await _uow.ExecuteAsync(async () =>
        {
            var normalizeEmail = request.Email.ToLowerInvariant();
            
            var checkEmailExist = await _identityService.CheckEmailExist(normalizeEmail, cancellationToken);

            if (checkEmailExist.IsFailure)
            {
                // Don't reveal that the user does not exist for security reasons
                return Result.Success();
            }

            var getUserIdResult = await _identityService.GetUserIdByEmail(normalizeEmail, cancellationToken);
            if (getUserIdResult.IsFailure)
            {
                return Result.Success();
            }

            var getTokenResult = await _identityService.GeneratePasswordResetToken(normalizeEmail, cancellationToken);
            if (getTokenResult.IsFailure)
            {
                return getTokenResult;
            }

            var resetUrl = $"{ResetPasswordUrl}?userId={getUserIdResult.Value}&token={Uri.EscapeDataString(getTokenResult.Value)}";

            await _eventBus.PublishAsync(
                new ForgotPasswordIntegrationEvent(
                    Guid.NewGuid(),
                    DateTime.UtcNow,
                    normalizeEmail,
                    getUserIdResult.Value,
                    getTokenResult.Value,
                    resetUrl
                ), cancellationToken);

            return Result.Success();
        });
    }
}
