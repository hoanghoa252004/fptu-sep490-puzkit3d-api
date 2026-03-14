using PuzKit3D.Modules.Notification.Application.Services;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Notification.Application.UseCases.Emails.Commands.SendEmailCommand;

public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand>
{
    private readonly IEmailService _emailService;
    
    public SendEmailCommandHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task<Result> Handle(SendEmailCommand request, CancellationToken cancellationToken)
    {
        return await _emailService.SendAsync(request.ToEmail, request.Subject, request.Body);
    }
}
