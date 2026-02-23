using PuzKit3D.SharedKernel.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Application.Exceptions;

public sealed class PuzKit3DException : Exception
{
    public PuzKit3DException(
        string requestName,
        Error? error = default,
        Exception? innerException = default
    ) : base(requestName, innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }

    public Error? Error { get; }
}
