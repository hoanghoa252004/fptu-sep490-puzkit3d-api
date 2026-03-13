using Microsoft.AspNetCore.Http;
using PuzKit3D.SharedKernel.Application.Message.Command;

namespace PuzKit3D.Modules.Payment.Application.UseCases.Transactions.Commands.CreateTransaction;

public sealed record CreateTransactionCommand(HttpContext context, Guid paymentId, string provider) : ICommandT<string>;

