using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Options;
using PuzKit3D.Modules.Notification.Application.Services;
using PuzKit3D.Modules.Notification.Domain.Emails;
using PuzKit3D.Modules.Notification.Infrastructure.DependencyInjection.Options;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Notification.Infrastructure.Services;

internal sealed class AwsSesEmailService : IEmailService
{
    private readonly IAmazonSimpleEmailService _ses;
    private readonly EmailSettings _emailSettings;

    public AwsSesEmailService(IAmazonSimpleEmailService ses, IOptions<EmailSettings> options)
    {
        _ses = ses;
        _emailSettings = options.Value;
    }
    public async Task<Result> SendAsync(string toEmail, string subject, string body)
    {
        var request = new SendEmailRequest
        {
            Source = _emailSettings.SenderEmail,
            Destination = new Destination
            {
                ToAddresses = [toEmail]
            },
            Message = new Message
            {
                Subject = new Content(subject),
                Body = new Body
                {
                    Html = new Content(body)
                }
            }
        };

        var response = await _ses.SendEmailAsync(request);

        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            return Result.Failure(EmailError.FailedSendEmail());

        return Result.Success();
    }
}
