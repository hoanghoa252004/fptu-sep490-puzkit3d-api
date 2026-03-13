using Microsoft.AspNetCore.Http;
using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Payment.Application.UseCases.Payments;

public sealed record CreatePaymentUrlCommand(HttpContext context, Guid OrderId, string provider) : ICommandT<string>;

