using PuzKit3D.Contract.User;
using PuzKit3D.Modules.Notification.Application.Services;
using PuzKit3D.SharedKernel.Application.Event;

namespace PuzKit3D.Modules.Notification.Infrastructure.IntegrationEventHandlers;

internal sealed class ForgotPasswordIntegrationEventHandler : IIntegrationEventHandler<ForgotPasswordIntegrationEvent>
{
    private readonly IEmailService _emailService;

    public ForgotPasswordIntegrationEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task HandleAsync(ForgotPasswordIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        await _emailService.SendResetPasswordEmailAsync(@event.Email, @event.ResetUrl);
    }
}
