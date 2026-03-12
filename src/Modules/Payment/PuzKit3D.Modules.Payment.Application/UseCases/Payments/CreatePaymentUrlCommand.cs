using PuzKit3D.Modules.Payment.Application.Abstractions;
using PuzKit3D.SharedKernel.Application.Message.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PuzKit3D.Modules.Payment.Application.UseCases.Payments;

public sealed record CreatePaymentUrlCommand : ICommandT<string>
{
}
