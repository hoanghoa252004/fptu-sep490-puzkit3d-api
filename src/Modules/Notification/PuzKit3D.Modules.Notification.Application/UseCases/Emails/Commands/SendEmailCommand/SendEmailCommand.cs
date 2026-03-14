using PuzKit3D.SharedKernel.Application.Message.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Notification.Application.UseCases.Emails.Commands.SendEmailCommand;

public sealed record SendEmailCommand(
    string ToEmail,
    string Subject,
    string Body ) : ICommand;
