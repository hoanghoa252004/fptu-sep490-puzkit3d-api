using MediatR;
using PuzKit3D.SharedKernel.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Message.Query;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, ResultT<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
