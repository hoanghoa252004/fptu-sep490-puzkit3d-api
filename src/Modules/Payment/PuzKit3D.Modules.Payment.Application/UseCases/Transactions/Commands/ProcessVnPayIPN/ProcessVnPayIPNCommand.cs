using MediatR;
using Microsoft.AspNetCore.Http;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Payment.Application.UseCases.Transactions.Commands.ProcessVnPayIPN;

public record ProcessVnPayIPNCommand(
    IQueryCollection QueryParameters) : IRequest<Result>;
