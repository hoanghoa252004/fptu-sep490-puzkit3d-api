using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Modules.Notification.Infrastructure.DependencyInjection.Options;

public sealed class EmailSettings
{
    public const string ConfigurationSection = nameof(EmailSettings);
    public string SenderEmail { get; set; } = string.Empty;
}
