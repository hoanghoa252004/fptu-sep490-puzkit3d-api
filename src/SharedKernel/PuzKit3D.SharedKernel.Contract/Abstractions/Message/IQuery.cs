using MediatR;
using PuzKit3D.Contract.Abstractions.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Contract.Abstractions.Message;

public interface IQuery : IRequest<Result> { }

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }