using PuzKit3D.Contract.Abstractions.Shared.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.Application.Exceptions;

public sealed class PuzKit3DException : Exception
{
    public PuzKit3DException(
        string requestName,
        string message,
        Error? error = default,
        Exception? innerException = default
    ) : base(message, innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }

    public Error? Error { get; }
}
