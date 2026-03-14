using PuzKit3D.Contract.User;
using PuzKit3D.Modules.Notification.Application.Services;
using PuzKit3D.SharedKernel.Application.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Notification.Infrastructure.IntegrationEventHandlers;

internal sealed class UserRegisteredIntegrationEventHandler : IIntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    private readonly IEmailService _emailService;

    public UserRegisteredIntegrationEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task HandleAsync(UserRegisteredIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        await _emailService.SendVerifyEmailAsync(@event.Email, @event.Token);
    }
}
