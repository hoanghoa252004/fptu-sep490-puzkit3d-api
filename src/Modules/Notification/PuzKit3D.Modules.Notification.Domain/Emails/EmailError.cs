using PuzKit3D.SharedKernel.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Notification.Domain.Emails;

public static class EmailError
{
    public static Error FailedSendEmail() => Error.Failure(
        "Email.SendEmail",
        "Sending email operation failed by some reason");
}
