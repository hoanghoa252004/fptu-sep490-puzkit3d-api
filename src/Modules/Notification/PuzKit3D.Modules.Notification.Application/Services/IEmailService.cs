using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Notification.Application.Services;

public interface IEmailService
{
    Task<Result> SendAsync(string toEmail, string subject, string body);
}
