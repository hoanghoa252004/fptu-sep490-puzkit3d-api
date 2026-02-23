using MediatR;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Message.Command;

public interface ICommandTHandler<in TCommand, TResponse> : IRequestHandler<TCommand, ResultT<TResponse>>
    where TCommand : ICommandT<TResponse>
{
}