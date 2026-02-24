using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PuzKit3D.SharedKernel.Application.Data;
using PuzKit3D.SharedKernel.Application.Message.Command;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Behaviors;

internal sealed class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IServiceProvider _serviceProvider;

    public TransactionBehavior(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!IsCommand()) // Query request
            return await next();

        var unitOfWork = _serviceProvider.GetService<IUnitOfWork>();

        if (unitOfWork is null)
            return await next();

        return await unitOfWork.ExecuteAsync(() => next());

    }

    private static bool IsCommand()
    {
        return typeof(TRequest).GetInterfaces()
            .Any(i => i.IsGenericType && (
                i.GetGenericTypeDefinition() == typeof(ICommandT<>) ||
                i == typeof(ICommand)));
    }
}
